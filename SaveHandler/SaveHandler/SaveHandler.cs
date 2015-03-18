using System;
using System.Collections;
using System.Xml.Serialization;
using System.IO;

namespace Handler.Save
{
    public class SaveHandler
    {
        #region Variables

        /// <summary>
        /// Contains all loaded files.
        /// </summary>
        private static Hashtable m_LoadedFiles = null;

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        public SaveHandler()
        {
            m_LoadedFiles = new Hashtable();
        }

        /// <summary>
        /// Deconstructor
        /// </summary>
        ~SaveHandler()
        {
            if (m_LoadedFiles != null)
            {
                m_LoadedFiles = null;
            }
        }

        #endregion

        #region Gets, Adds, Removes, Deletes

        /// <summary>
        /// Get a loaded file from
        /// the hashtable. Will return
        /// null if the file doesn't exist.
        /// </summary>
        /// <typeparam name="T">
        /// The files type.
        /// </typeparam>
        /// <param name="location">
        /// The Location of the file.
        /// </param>
        /// <returns></returns>
        public T GetFile<T>(string location)
        {

            // Check if the file is loaded.
            T file = GetLoadedFile<T>(location);

            // Check if the file is
            // loaded and return it.
            if (file != null)
            {
                return file;
            }

            // File isn't loaded. Attempt 
            // to load the file.
            return LoadFile<T>(location);
        }

        /// <summary>
        /// Removes loaded file
        /// from the hashtable.
        /// </summary>
        /// <param name="location">
        /// The location of the file.
        /// </param>
        public void RemoveLoadedFile(string location)
        {
            m_LoadedFiles.Remove(location);
        }

        /// <summary>
        /// Removes file if loaded,
        /// and deletes it.
        /// </summary>
        /// <param name="location">
        /// The location of the file.
        /// </param>
        public void DeleteFile(string location)
        {
            m_LoadedFiles.Remove(location);
            File.Delete(location);
        }

        /// <summary>
        /// Get a loaded file
        /// from the hashtable.
        /// </summary>
        /// <typeparam name="T">
        /// The files type.
        /// </typeparam>
        /// <param name="location">
        /// The location of the file.
        /// </param>
        /// <returns>
        /// Returns the file from
        /// the hashtable.
        /// </returns>
        private T GetLoadedFile<T>(string location)
        {
            return (T)m_LoadedFiles[location];
        }

        /// <summary>
        /// Add the loaded file
        /// to the hashtable.
        /// </summary>
        /// <typeparam name="T">
        /// The files type.
        /// </typeparam>
        /// <param name="location">
        /// The location of the file.
        /// </param>
        /// <param name="file">
        /// The file that is loaded.
        /// </param>
        private void AddLoadedFile<T>(string location, T file)
        {
            m_LoadedFiles.Add(location, file);
        }

        #endregion

        #region Loads, Saves, Creates

        /// <summary>
        /// Creates a new file. Will return false
        /// if the file already exists.
        /// </summary>
        /// <typeparam name="T">
        /// The files type.
        /// </typeparam>
        /// <param name="location">
        /// Location of the file.
        /// </param>
        /// <param name="file">
        /// The file that will be created.
        /// </param>
        /// <param name="overwrite">
        /// Will an existing file be overwritten?
        /// </param>
        /// <returns>
        /// Returns true if the file was succesfully
        /// created and saved.
        /// </returns>
        public bool CreateFile<T>(string location, T file, bool overwrite)
        {
            // Check if the file exists or if 
            // the file is being overwritten.
            if (File.Exists(location) == false || overwrite == true)
            {
                // Add the file to the loaded
                // table and save it.
                AddLoadedFile<T>(location, file);
                return SaveFile<T>(location);
            }

            // File already exists or
            // cannot be overwritten.
            return false;
        }

        /// <summary>
        /// Saves the loaded file. Will return
        /// false if the file is not loaded,
        /// or failed to save.
        /// </summary>
        /// <typeparam name="T">
        /// The files type.
        /// </typeparam>
        /// <param name="location">
        /// Location of the file.
        /// </param>
        /// <returns>
        /// Returns true if the file 
        /// was succesfully saved.
        /// </returns>
        public bool SaveFile<T>(string location)
        {
            // Get the file that will be saved
            T file = GetFile<T>(location);

            // Check if the file exists
            if (file != null)
            {
                try
                {

                    // Initialize a writer and
                    // stream to the file.
                    XmlSerializer writer = new XmlSerializer(typeof(T));
                    StreamWriter stream = new StreamWriter(location);

                    // Attempt to serialize the file
                    // and close after completion.
                    writer.Serialize(stream, file);
                    stream.Close();

                    return true;
                }
                catch (Exception /*e*/)
                {
                }
            }

            // File is not loaded, or
            // failed to save.
            return false;
        }

        /// <summary>
        /// Loads the file. Returns 
        /// the file if its found.
        /// </summary>
        /// <typeparam name="T">
        /// The files type.
        /// </typeparam>
        /// <param name="location">
        /// Location of the file.
        /// </param>
        /// <returns>
        /// Returns null if the file
        /// cannot be found.
        /// </returns>
        private T LoadFile<T>(string location)
        {
            try
            {

                // Initialize a reader and 
                // stream to the file.
                XmlSerializer reader = new XmlSerializer(typeof(T));
                StreamReader stream = new StreamReader(location);

                // Attempt to Deserialize the 
                // stream, add it to the loaded 
                // files table.
                T file = (T)reader.Deserialize(stream);
                AddLoadedFile<T>(location, file);

                // Close the stream, and 
                // return the file.
                stream.Close();
                return file;
            }
            catch (Exception /*e*/)
            {
            }

            // Failed to load the file.
            // File may not exist.
            return default(T);
        }

        #endregion
    }

    /// <summary>
    /// Default XmlFile. This can be used
    /// as a base class.
    /// </summary>
    [Serializable]
    public class XmlFile
    {
        [XmlAttribute]
        public string Title = string.Empty;

        public XmlFile() { }

        public XmlFile(string title)
        {
            Title = title;
        }

        ~XmlFile() { }
    }
}
