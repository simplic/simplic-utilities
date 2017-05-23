using Simplic.ICR.Selector;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Simplic.ICR
{
    /// <summary>
    /// Main analyser class, to get all tags for a tags and produce an ICRObject.
    /// </summary>
    public class Analyser
    {
        #region Fields

        private IList<Tag> tags;

        #endregion Fields

        #region Constructor

        /// <summary>
        /// Create a new instance of the ICR analyser
        /// </summary>
        public Analyser()
        {
            Clear();
        }

        #endregion Constructor

        #region Public Methods

        /// <summary>
        /// Add a tag via tag configuration string
        /// </summary>
        /// <param name="text">Name of the Tag</param>
        /// <param name="id">Unique id of the string</param>
        /// <param name="configuration">Configuration string, selector</param>
        /// <returns>Instance of the created tag</returns>
        public Tag AddTag(string text, Guid id, string configuration)
        {
            if (text == null)
            {
                throw new ArgumentNullException("text");
            }
            if (configuration == null)
            {
                throw new ArgumentNullException("configuration");
            }

            Tag tag = new Tag(text, id);

            string[] splitted = configuration.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);

            Filter filter = null;

            foreach (var split in splitted)
            {
                string toProcess = split;
                ISelector selector = null;

                if (toProcess.StartsWith("\t") || toProcess.StartsWith(" "))
                {
                    toProcess = toProcess.Trim();
                }
                else
                {
                    filter = new Filter();
                    tag.AddFilter(filter);
                }

                if (toProcess.StartsWith("rx:"))
                {
                    selector = new RegexSelector();
                    selector.Initialize(toProcess.Substring(3, toProcess.Length - 3));
                }
                else if (toProcess.StartsWith("icr:"))
                {
                    selector = new PercentageTextSelector();
                    selector.Initialize(toProcess.Substring(4, toProcess.Length - 4));
                }
                else
                {
                    throw new Exception("Unexpected line start: " + toProcess + ". Configuration: " + configuration);
                }

                // Add selector
                filter.AddSelector(selector);
            }

            this.tags.Add(tag);
            return tag;
        }

        /// <summary>
        /// Clear and initialize the analyser
        /// </summary>
        public void Clear()
        {
            tags = new List<Tag>();
        }

        /// <summary>
        /// Analyse a text amd return an ICRObject containing all result information
        /// </summary>
        /// <param name="text">Input text</param>
        /// <param name="detectAllTags">Detect a tag, even if it was already matched, for tag completion in the ICRObject.</param>
        /// <returns>Instance of an ICRObject with all analying information</returns>
        public ICRObject Analye(string text, bool detectAllTags = true)
        {
            if (text == null)
            {
                throw new ArgumentNullException("text");
            }

            // None alphanumeric chars
            Regex rgx = new Regex("[^a-zA-Z0-9 -]");
            text = rgx.Replace(text, " ");

            ICRObject returnValue = new ICRObject();

            // Split the text
            string[] splittedText = text.Split(new string[] { " ", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries);

            foreach (Tag tag in Tags)
            {
                bool doAssignTag = false;

                IList<string> matches = new List<string>();

                foreach (Filter filter in tag.Filter)
                {
                    bool selectorMatch = true;

                    foreach (ISelector selector in filter.Selector)
                    {
                        bool textMatch = false;

                        string _out = "";

                        foreach (string part in splittedText)
                        {
                            // Execute selector
                            if (selector.Compare(part, out _out))
                            {
                                textMatch = true;
                                matches.Add(_out);

                                // leave text iterator
                                if (!detectAllTags)
                                {
                                    break;
                                }
                            }
                        }

                        if (!textMatch)
                        {
                            selectorMatch = false;

                            // break selector iterator
                            if (!detectAllTags)
                            {
                                break;
                            }
                        }
                    }

                    // Leave if a filter match, because the tag will be assigned
                    if (selectorMatch)
                    {
                        doAssignTag = true;
                        if (!detectAllTags)
                        {
                            break;
                        }
                    }
                }

                if (doAssignTag)
                {
                    var assignTag = new AssignedTag() { Text = tag.Text, Id = tag.Id, Matches = matches };
                    returnValue.Tags.Add(assignTag);
                }
            }

            return returnValue;
        }

        #endregion Public Methods

        #region Public Member

        /// <summary>
        /// List with all available tags
        /// </summary>
        public IList<Tag> Tags
        {
            get { return tags; }
        }

        #endregion Public Member
    }
}