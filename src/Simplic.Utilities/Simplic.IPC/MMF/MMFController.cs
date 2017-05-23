using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Linq;
using System.Text;
using System.Threading;

namespace Simplic.IPC.MMF
{
    /// <summary>
    /// Controller for sending and retrieving data over Memory Mapped Files (MMF).
    /// Implements the IIPCController interface, to be a full qualifierd ipc controller
    ///
    /// 1. Every controller provides a mmf, in which another controller can write. This mmf contains all commands
    ///    The providing controller can receive
    ///
    /// </summary>
    public class MMFController : IIPCController
    {
        #region [Const]

        /// <summary>
        /// Life-time of a message (MMF) in Milliseconds
        /// </summary>
        public const int RECEIVE_DATA_TIMEOUT = 5000;

        /// <summary>
        /// Milliseconds how long mutex lock a thread
        /// </summary>
        public const int MUTEX_THREAD_BLOCKING = 100;

        #endregion [Const]

        #region [Statis Methods]

        /// <summary>
        /// Read all bytes from a binary reader
        /// </summary>
        /// <param name="reader">Instance of a binrary reader</param>
        /// <returns>Return a byte array containing the complete binary reader</returns>
        public static byte[] ReadAllBytes(BinaryReader reader)
        {
            const int bufferSize = 4096;
            using (var ms = new MemoryStream())
            {
                byte[] buffer = new byte[bufferSize];
                int count;
                while ((count = reader.Read(buffer, 0, buffer.Length)) != 0)
                {
                    ms.Write(buffer, 0, count);
                }
                return ms.ToArray();
            }
        }

        #endregion [Statis Methods]

        #region Singleton

        private static readonly MMFController singleton = new MMFController();

        /// <summary>
        /// Singleton for accessing the controller
        /// </summary>
        public static MMFController Singleton
        {
            get
            {
                return singleton;
            }
        }

        #endregion Singleton

        #region Private Member

        private IDictionary<string, Tuple<Type, OnExecuteCommand>> commands;
        private bool isInitialized;
        private IPCConfiguration configuration;
        private OnExecuteErrorCommand errorHandler;
        private IDictionary<string, IPCConfiguration> clients;
        private MemoryMappedFile communicationMap;
        private string communicationMapName;
        private IDictionary<Guid, MemoryMappedFile> sendedFiles;
        private Mutex communicationMapMutex;

        #endregion Private Member

        #region Constructor

        /// <summary>
        /// Create a new controller
        /// </summary>
        public MMFController()
        {
            commands = new Dictionary<string, Tuple<Type, OnExecuteCommand>>();
            clients = new Dictionary<string, IPCConfiguration>();
            sendedFiles = new Dictionary<Guid, MemoryMappedFile>();
            isInitialized = false;
        }

        #endregion Constructor

        #region Public Methods

        #region [Initialize]

        /// <summary>
        /// Initialize the current controller
        /// </summary>
        /// <param name="configuration">Current controller configuration</param>
        /// <param name="errorHandler">Handler for dealing with errors</param>
        public void Initialize(IPCConfiguration configuration, OnExecuteErrorCommand errorHandler)
        {
            if (configuration == null)
            {
                throw new ArgumentNullException("configuration");
            }
            if (errorHandler == null)
            {
                throw new ArgumentNullException("errorHandler");
            }

            // Create unique Session-Id
            configuration.Name += "_" + Simplic.Win32.WindowsSession.GetSid();

            this.configuration = configuration;
            this.errorHandler = errorHandler;

            // Initialize MMF
            communicationMapName = configuration.Name;

            bool fileAlreadyExists = false;

            // Proof whether the file is already in use
            try
            {
                MemoryMappedFile.OpenExisting(communicationMapName);
                fileAlreadyExists = true;
            }
            catch (FileNotFoundException)
            {
                fileAlreadyExists = false;
            }

            // The file should not exists
            if (fileAlreadyExists == true)
            {
                throw new Exception("Client already exists: " + communicationMapName);
            }
            else
            {
                // Create new maped file
                communicationMap = MemoryMappedFile.CreateNew(communicationMapName, 1048576);

                IPCCommunicationMap defaultComMap = new IPCCommunicationMap();
                defaultComMap.ReadedCommands = defaultComMap.ReadedCommands ?? new List<CommandItem>();
                defaultComMap.SendedCommands = defaultComMap.SendedCommands ?? new List<CommandItem>();

                BinaryWriter writer = new BinaryWriter(communicationMap.CreateViewStream());
                writer.Write(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(defaultComMap)));

                bool createdNew;
                communicationMapMutex = new Mutex(false, communicationMapName + "_mutex_lock", out createdNew);

                Console.WriteLine("Create new: " + createdNew.ToString());

                isInitialized = true;
            }
        }

        #endregion [Initialize]

        #region [AddClient]

        /// <summary>
        /// Add an MMF client to the current controller instance for sending and retrieving data
        /// </summary>
        /// <param name="configuration">Instance of the client configuration</param>
        public void AddClient(IPCConfiguration configuration)
        {
            if (configuration == null)
            {
                throw new ArgumentNullException("configuration");
            }
            if (!isInitialized)
            {
                throw new Exception("MMF controller is not initialized (ipc)");
            }

            if (configuration.Name.EndsWith(Simplic.Win32.WindowsSession.GetSid()) == false)
            {
                configuration.Name += "_" + Simplic.Win32.WindowsSession.GetSid();
            }

            if (!clients.ContainsKey(configuration.Name))
            {
                clients.Add(configuration.Name, configuration);
            }
            else
            {
                var arg = new IPCErrorEventArgs();
                arg.ErrorMessage = "Client already connected: " + configuration.Name;

                // Call Error-Handler
                errorHandler.Invoke(this, arg);
            }
        }

        #endregion [AddClient]

        #region [AddCommand]

        /// <summary>
        /// Add a new command to the current communicationMap controller
        /// </summary>
        /// <typeparam name="T">Instance of a command which inherit from IPCCommand</typeparam>
        /// <param name="cmdName">Unique name of the command</param>
        /// <param name="handler">Handler to deal with incomming messages</param>
        public void AddCommand<T>(string cmdName, OnExecuteCommand handler) where T : IPCCommand
        {
            if (string.IsNullOrWhiteSpace(cmdName))
            {
                throw new ArgumentOutOfRangeException("cmdName");
            }
            if (handler == null)
            {
                throw new ArgumentNullException("handler");
            }
            if (!isInitialized)
            {
                throw new Exception("MMF controller is not initialized (ipc)");
            }

            if (!commands.ContainsKey(cmdName))
            {
                commands.Add(cmdName, new Tuple<Type, OnExecuteCommand>(typeof(T), handler));
            }
            else
            {
                var arg = new IPCErrorEventArgs();
                arg.ErrorMessage = "Command is already existing: " + cmdName;

                // Call Error-Handler
                errorHandler.Invoke(this, arg);
            }
        }

        #endregion [AddCommand]

        #region [RemoveCommand]

        /// <summary>
        /// Remove a command from the current controller
        /// </summary>
        /// <param name="cmdName">Unique name of the command</param>
        public void RemoveCommand(string cmdName)
        {
            if (string.IsNullOrWhiteSpace(cmdName))
            {
                throw new ArgumentOutOfRangeException("cmdName");
            }
            if (!isInitialized)
            {
                throw new Exception("MMF controller is not initialized (ipc)");
            }

            if (commands.ContainsKey(cmdName))
            {
                commands.Remove(cmdName);
            }
            else
            {
                var arg = new IPCErrorEventArgs();
                arg.ErrorMessage = "Command can not be remove, becuase it does not exists: " + cmdName;

                // Call Error-Handler
                errorHandler.Invoke(this, arg);
            }
        }

        #endregion [RemoveCommand]

        #region [WriteCommand]

        /// <summary>
        /// Write a command to the mmf
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="packId">Id of the package</param>
        /// <param name="obj">Object instance</param>
        private void WriteCommand<T>(Guid packId, T obj) where T : IPCCommand
        {
            byte[] result = Encoding.UTF8.GetBytes(Newtonsoft.Json.JsonConvert.SerializeObject(obj));
            MemoryMappedFile sendedMMF = MemoryMappedFile.CreateNew(packId.ToString(), result.Length);

            using (MemoryMappedViewStream stream = sendedMMF.CreateViewStream())
            {
                BinaryWriter writer = new BinaryWriter(stream);
                writer.Write(result);
            }

            sendedFiles.Add(packId, sendedMMF);
        }

        #endregion [WriteCommand]

        #region [SendCommandAsync]

        /// <summary>
        /// Send a command to an existing client
        /// </summary>
        /// <typeparam name="T">Type of the command</typeparam>
        /// <param name="obj">Instance of the command</param>
        /// <param name="client">Unique name of the destination client</param>
        public void SendCommandAsync<T>(T obj, string client) where T : IPCCommand
        {
            if (obj == default(T))
            {
                throw new ArgumentNullException("obj");
            }
            if (string.IsNullOrWhiteSpace(client))
            {
                throw new ArgumentOutOfRangeException("client");
            }
            if (!isInitialized)
            {
                throw new Exception("MMF controller is not initialized (ipc)");
            }

            if (clients.ContainsKey(client))
            {
                IPCConfiguration clientConfiguration = clients[client];

                // Create a message id
                Guid id = Guid.NewGuid();

                // Get map from client
                string clientMap = client;

                // Proof whether the clientMap exists
                try
                {
                    using (MemoryMappedFile clientMMF = MemoryMappedFile.OpenExisting(clientMap))
                    {
                        // =======================================================
                        // Create new Memory mapped file containing all data to send
                        WriteCommand<T>(id, obj);
                        // =======================================================

                        // Lock client map
                        Mutex mutex = null;

                        if (Mutex.TryOpenExisting(clientMap + "_mutex_lock", out mutex))
                        {
                            if (mutex.WaitOne())
                            {
                                try
                                {
                                    // Read map from client
                                    BinaryReader reader = new BinaryReader(clientMMF.CreateViewStream());
                                    // Int max is allowed, cause: The documentation says that it will read all bytes until the end of the stream is reached.
                                    string jsonObj = Encoding.UTF8.GetString(ReadAllBytes(reader));

                                    IPCCommunicationMap deserializedMap = JsonConvert.DeserializeObject<IPCCommunicationMap>(jsonObj);
                                    deserializedMap.ReadedCommands = deserializedMap.ReadedCommands ?? new List<CommandItem>();
                                    deserializedMap.SendedCommands = deserializedMap.SendedCommands ?? new List<CommandItem>();

                                    reader.Close();

                                    #region [ Remove messages with time out]

                                    var mmfToRemoveOverTimeout = deserializedMap.SendedCommands.Where(Item => Item.TimeStamp + RECEIVE_DATA_TIMEOUT < System.Environment.TickCount).ToList();
                                    foreach (var toRemove in mmfToRemoveOverTimeout)
                                    {
                                        try
                                        {
                                            if (sendedFiles.ContainsKey(toRemove.PackageId))
                                            {
                                                sendedFiles[toRemove.PackageId].Dispose();
                                            }
                                        }
                                        catch (FileNotFoundException)
                                        {
                                            // Do nothing
                                        }

                                        // Remove from sended list, this should work, because CommandItem overrides GetHashCode and Equals
                                        Console.WriteLine("Delete due to time out: " + toRemove.CommandName);
                                        deserializedMap.SendedCommands.Remove(toRemove);
                                    }

                                    #endregion [ Remove messages with time out]

                                    // Add new command to map from the client
                                    CommandItem item = new CommandItem();
                                    item.PackageId = id;
                                    item.TimeStamp = System.Environment.TickCount;
                                    item.CommandName = obj.CommandName;
                                    item.SenderName = configuration.Name;

                                    Console.WriteLine("Sended pack: {0} - {1}", item.CommandName, item.PackageId);

                                    deserializedMap.SendedCommands.Add(item);

                                    // Remove readed messages from the client
                                    foreach (var readedFile in deserializedMap.ReadedCommands)
                                    {
                                        try
                                        {
                                            if (sendedFiles.ContainsKey(readedFile.PackageId))
                                            {
                                                sendedFiles[readedFile.PackageId].Dispose();
                                            }
                                        }
                                        catch (FileNotFoundException)
                                        {
                                            // Do nothing
                                        }
                                    }
                                    deserializedMap.ReadedCommands = new List<CommandItem>();

                                    string serializedObject = JsonConvert.SerializeObject(deserializedMap);

                                    Simplic.Log.LogManagerInstance.Instance.Debug(client + "@Communication-Map: \r\n" + serializedObject, null, null, "ipc");

                                    // Save map from the client, so he can receive the data
                                    BinaryWriter writer = new BinaryWriter(clientMMF.CreateViewStream());
                                    writer.Write(Encoding.UTF8.GetBytes(serializedObject));
                                }
                                finally
                                {
                                    mutex.ReleaseMutex();
                                }
                            }
                        }
                        else
                        {
                            var arg = new IPCErrorEventArgs();
                            arg.ErrorMessage = "Could not open mutex for sending data: " + client + "/" + clientMap;

                            // Call Error-Handler
                            errorHandler.Invoke(this, arg);
                        }
                        // =======================================================
                    }
                }
                catch (FileNotFoundException)
                {
                    var arg = new IPCErrorEventArgs();
                    arg.ErrorMessage = "Client communication map file not found: " + client + "/" + clientMap;

                    // Call Error-Handler
                    errorHandler.Invoke(this, arg);
                }
                catch (Exception ex)
                {
                    var arg = new IPCErrorEventArgs();
                    arg.ErrorMessage = String.Format("Error in receive method: " + ex.ToString());

                    // Call Error-Handler
                    errorHandler.Invoke(this, arg);
                }
            }
            else
            {
                var arg = new IPCErrorEventArgs();
                arg.ErrorMessage = "Client configuration could not be found for: " + client;

                // Call Error-Handler
                errorHandler.Invoke(this, arg);
            }
        }

        #endregion [SendCommandAsync]

        #region [Read and execute command]

        /// <summary>
        /// Read command and execute
        /// </summary>
        private void ReadCommand(CommandItem command)
        {
            try
            {
                // Read command and Execute commands
                using (MemoryMappedFile clientMMF = MemoryMappedFile.OpenExisting(command.PackageId.ToString()))
                {
                    // =======================================================
                    // Opem memory mapped files
                    MemoryMappedFile packMMF = MemoryMappedFile.OpenExisting(command.PackageId.ToString());

                    using (MemoryMappedViewStream stream = packMMF.CreateViewStream())
                    {
                        // Read object
                        BinaryReader packReader = new BinaryReader(packMMF.CreateViewStream());

                        // Int max is allowed, cause: The documentation says that it will read all bytes until the end of the stream is reached.
                        string packJsonObj = Encoding.UTF8.GetString(ReadAllBytes(packReader));

                        object deserializedCommand = JsonConvert.DeserializeObject(packJsonObj);

                        //
                        var commandOptions = commands[command.CommandName];

                        var arg = new IPCReceiveEventArgs();
                        arg.JSON = packJsonObj;
                        arg.CommandName = command.CommandName;

                        // Call Error-Handler
                        commandOptions.Item2.Invoke(this, arg);
                    }
                    // =======================================================
                }
            }
            catch (FileNotFoundException)
            {
                var arg = new IPCErrorEventArgs();
                arg.ErrorMessage = String.Format("Received pack not found: {0} - {1}", command.CommandName, command.PackageId);

                // Call Error-Handler
                errorHandler.Invoke(this, arg);
            }
            catch (Exception ex)
            {
                var arg = new IPCErrorEventArgs();
                arg.ErrorMessage = String.Format("Could not read or execute command: {0} - {1}: {2} / {3}", command.CommandName, command.PackageId, ex.Message, (ex.InnerException == null ? "" : ex.InnerException.ToString()));

                // Call Error-Handler
                errorHandler.Invoke(this, arg);
            }
        }

        #endregion [Read and execute command]

        /// <summary>
        /// Receive all available data
        /// </summary>
        public void Receive()
        {
            if (!isInitialized)
            {
                throw new Exception("MMF controller is not initialized (ipc)");
            }

            // Reponse messages
            IDictionary<string, IList<Guid>> response = new Dictionary<string, IList<Guid>>();

            // Wait 200ms, if waitone was successfull, read from the file
            try
            {
                if (communicationMapMutex.WaitOne(MUTEX_THREAD_BLOCKING))
                {
                    try
                    {
                        // Read communiactionMap
                        BinaryReader reader = new BinaryReader(communicationMap.CreateViewStream());
                        // Int max is allowed, cause: The documentation says that it will read all bytes until the end of the stream is reached.
                        string jsonObj = Encoding.UTF8.GetString(ReadAllBytes(reader));

                        IPCCommunicationMap deserializedMap = JsonConvert.DeserializeObject<IPCCommunicationMap>(jsonObj);
                        deserializedMap.ReadedCommands = deserializedMap.ReadedCommands ?? new List<CommandItem>();
                        deserializedMap.SendedCommands = deserializedMap.SendedCommands ?? new List<CommandItem>();

                        // Read new messages
                        foreach (var command in deserializedMap.SendedCommands)
                        {
                            // Rad and execute command
                            ReadCommand(command);

                            // Response commands should not be added for responding
                            if (command.CommandName != "IPCReposonseCommand")
                            {
                                if (!response.ContainsKey(command.SenderName))
                                {
                                    List<Guid> list = new List<Guid>();
                                    list.Add(command.PackageId);
                                    response.Add(command.SenderName, list);
                                }
                                else
                                {
                                    response[command.SenderName].Add(command.PackageId);
                                }
                            }

                            // Set command as readed
                            deserializedMap.ReadedCommands.Add(command);
                        }

                        deserializedMap.SendedCommands = new List<CommandItem>();

                        // Save map
                        BinaryWriter writer = new BinaryWriter(communicationMap.CreateViewStream());
                        writer.Write(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(deserializedMap)));
                    }
                    finally
                    {
                        communicationMapMutex.ReleaseMutex();
                    }
                }
            }
            catch (Exception ex)
            {
                var arg = new IPCErrorEventArgs();
                arg.ErrorMessage = String.Format("Mutex WaitOne not available: " + communicationMapName + "_mutex_lock:\r\n" + ex.ToString());

                // Call Error-Handler
                errorHandler.Invoke(this, arg);
            }

            // Wait for mutex
            Thread.Sleep(1);

            foreach (var respo in response)
            {
                if (!clients.ContainsKey(respo.Key))
                {
                    IPCConfiguration temp = new IPCConfiguration();
                    temp.Name = respo.Key;

                    this.AddClient(temp);
                }

                Simplic.Log.LogManagerInstance.Instance.ResetDebugStopwatch("ipc");
                Simplic.Log.LogManagerInstance.Instance.Debug("Begin send response: " + respo.Key, null, null, "ipc");

                if (respo.Value != null)
                {
                    foreach (var msg in respo.Value)
                    {
                        // Create response message
                        IPCReposonseCommand cmd = new IPCReposonseCommand();
                        cmd.CommandId = msg;

                        // Send Response
                        SendCommandAsync<IPCReposonseCommand>(cmd, respo.Key);
                    }
                }

                Simplic.Log.LogManagerInstance.Instance.Debug("End send response: " + respo.Key, null, null, "ipc");
            }
        }

        #region Public Member

        /// <summary>
        /// Initialization status
        /// </summary>
        public bool IsInitialized
        {
            get
            {
                return isInitialized;
            }
        }

        #endregion Public Member
    }

    #endregion Public Methods
}