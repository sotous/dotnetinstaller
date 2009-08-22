using System;
using System.Collections.Generic;
using System.Text;
using CommandLine;
using System.IO;

namespace InstallerLib
{
    /// <summary>
    /// Linker arguments
    /// </summary>
    public class InstallerLinkerArguments
    {
        [Argument(ArgumentType.Required, HelpText = "Generated installer file", LongName = "Output", ShortName = "o")]
        public string output;
        [Argument(ArgumentType.Required, HelpText = "Template installer executable", LongName = "Template", ShortName = "t")]
        public string template;
        [Argument(ArgumentType.AtMostOnce, HelpText = "Picture for the banner", LongName = "Banner", ShortName = "b")]
        public string banner;
        [Argument(ArgumentType.Required, HelpText = "XML configuration file", LongName = "Configuration", ShortName = "c")]
        public string config;
        [Argument(ArgumentType.AtMostOnce, HelpText = "Embed support files", LongName = "Embed", ShortName = "e", DefaultValue = true)]
        public bool embed;
        [Argument(ArgumentType.AtMostOnce, HelpText = "Location of support files for embedding", LongName = "AppPath", ShortName = "a")]
        public string apppath;
        [Argument(ArgumentType.AtMostOnce, HelpText = "Verbose output", LongName = "Verbose", ShortName = "v")]
        public bool verbose;
        [Argument(ArgumentType.MultipleUnique, HelpText = "Additional files to embed", LongName = "EmbedFile", ShortName = "f")]
        public string[] embedFiles;
        [Argument(ArgumentType.MultipleUnique, HelpText = "Additional folders, including subfolders to embed", LongName = "EmbedFolder", ShortName = "r")]
        public string[] embedFolders;
        [Argument(ArgumentType.AtMostOnce, HelpText = "Icon for the executable", LongName = "Icon", ShortName = "i")]
        public string icon;
        [Argument(ArgumentType.AtMostOnce, HelpText = "Embed manifest", LongName = "Manifest", ShortName = "m")]
        public string manifest;

        public void Validate()
        {
            // template
            if (string.IsNullOrEmpty(template))
                throw new ArgumentNullException("template");
            template = Path.GetFullPath(template);
            if (!File.Exists(template))
                throw new FileNotFoundException(template);

            // banner
            if (!string.IsNullOrEmpty(banner)) 
                banner = Path.GetFullPath(banner);
            if (!String.IsNullOrEmpty(banner) && !File.Exists(banner))
                throw new FileNotFoundException(banner);

            // icon
            if (!string.IsNullOrEmpty(icon))
            {
                icon = Path.GetFullPath(icon);
                if (!File.Exists(icon))
                    throw new FileNotFoundException(icon);
            }

            // config
            if (string.IsNullOrEmpty(config))
                throw new ArgumentNullException("config");
            config = Path.GetFullPath(config);
            if (!File.Exists(config))
                throw new FileNotFoundException(config);

            // output
            if (string.IsNullOrEmpty(output))
                throw new ArgumentNullException("output");
            output = Path.GetFullPath(output);

            // apppath
            if (!string.IsNullOrEmpty(apppath))
            {
                apppath = Path.GetFullPath(apppath);
                if (!Directory.Exists(apppath))
                    throw new DirectoryNotFoundException(apppath);
            }

            if (embedFiles != null)
            {
                foreach (string filename in embedFiles)
                {
                    string combinedFilename = Path.Combine(apppath, filename);
                    if (!File.Exists(combinedFilename))
                        throw new FileNotFoundException(combinedFilename);
                }
            }
        }

        public void WriteLine(string s)
        {
            if (verbose)
            {
                Console.WriteLine(s);
            }
        }

        public void WriteError(string s)
        {
            Console.Error.WriteLine(s);
        }
    }
}
