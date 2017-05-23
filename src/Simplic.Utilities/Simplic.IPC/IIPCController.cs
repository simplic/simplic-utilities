namespace Simplic.IPC
{
    /// <summary>
    /// Delegate which handles incomming messages
    /// </summary>
    /// <param name="sender">Object which sends the message</param>
    /// <param name="args">Message information</param>
    public delegate void OnExecuteCommand(object sender, IPCReceiveEventArgs args);

    /// <summary>
    /// Delegate which handles all errors, which may occures
    /// </summary>
    /// <param name="sender">Sender information</param>
    /// <param name="args">Argument containing all information about the error which are available</param>
    public delegate void OnExecuteErrorCommand(object sender, IPCErrorEventArgs args);

    /// <summary>
    /// IPCController which must be implemented in any controller type for inter process communication
    /// The first implementation will be in the Memory-Mapped-File controller. <see cref="MMF.MMFController"/>
    /// </summary>
    public interface IIPCController
    {
        /// <summary>
        /// Initialize a controller. This must only be done once per application start
        /// </summary>
        /// <param name="configuration">Configuration if the current controller</param>
        /// <param name="errorHandler">Default error handler, which will be called if any error occurse</param>
        void Initialize(IPCConfiguration configuration, OnExecuteErrorCommand errorHandler);

        /// <summary>
        /// Add a client by IPCConfiguration. The Name of the configuration must be unique
        /// </summary>
        /// <param name="configuration">Instance of an IPCConfiguration containing all information of the client</param>
        void AddClient(IPCConfiguration configuration);

        /// <summary>
        /// Add a new sendable and readable command
        /// </summary>
        /// <typeparam name="T">Type of the command, must inherit from IPCCommand</typeparam>
        /// <param name="cmdName">Name of the command, must be uniqe</param>
        /// <param name="handler">Handler which will be called, if a command was received</param>
        void AddCommand<T>(string cmdName, OnExecuteCommand handler) where T : IPCCommand;

        /// <summary>
        /// Remvoe a command by it's name
        /// </summary>
        /// <param name="cmdName">UNique name of the command</param>
        void RemoveCommand(string cmdName);

        /// <summary>
        /// Send a command to client.
        /// </summary>
        /// <typeparam name="T">Command type which will be send</typeparam>
        /// <param name="obj">Instance of an IPCCommand</param>
        /// <param name="client">Client to which the message will be send (Name of an IPCConfiguration)</param>
        void SendCommandAsync<T>(T obj, string client) where T : IPCCommand;
    }
}