using System;
using System.Windows.Forms;

namespace InstallerEditor
{
	public class TreeNodeConfigFile : TreeNode
	{
		public TreeNodeConfigFile(ConfigFile p_Config)
		{
			base.Text = "Config File";
			base.ImageIndex = 0;
			base.SelectedImageIndex = 0;

			m_ConfigFile = p_Config;

			base.Tag = p_Config;

			foreach (Configuration c in p_Config.Configurations)
				Nodes.Add(new TreeNodeConfiguration(c));
		}

		private ConfigFile m_ConfigFile;
		public ConfigFile ConfigFile
		{
			get{return m_ConfigFile;}
			set{m_ConfigFile = value;}
		}
	}

	public class TreeNodeConfiguration : TreeNode
	{
		public TreeNodeConfiguration(Configuration p_Config)
		{
			base.Text = p_Config.ToString();

			if (p_Config is WebConfiguration)
			{
				base.ImageIndex = 1;
				base.SelectedImageIndex = 1;

				Nodes.Add(new TreeNodeDownloadDialog( ((WebConfiguration)p_Config).DownloadDialog ) );
			}
			else if (p_Config is SetupConfiguration)
			{
				base.ImageIndex = 3;
				base.SelectedImageIndex = 3;

				foreach (Component c in ((SetupConfiguration)p_Config).Components)
				{
					Nodes.Add(new TreeNodeComponent(c));
				}
			}
			else
				throw new ApplicationException("Invalid configuration");

			m_Configuration = p_Config;

			base.Tag = p_Config;
			p_Config.LCIDChanged +=new EventHandler(p_Config_LCIDChanged);
		}

		private Configuration m_Configuration;
		public Configuration Configuration
		{
			get{return m_Configuration;}
			set{m_Configuration = value;}
		}

		private void p_Config_LCIDChanged(object sender, EventArgs e)
		{
			Text = Tag.ToString();
		}
	}

	public class TreeNodeDownloadDialog : TreeNode
	{
		public TreeNodeDownloadDialog(DownloadDialog p_Dialog)
		{
			base.Text = "Download Dialog";

			base.ImageIndex = 5;
			base.SelectedImageIndex = 5;

			m_DownloadDialog = p_Dialog;

			base.Tag = p_Dialog;

			foreach (Download d in p_Dialog.Downloads)
				Nodes.Add(new TreeNodeDownload(d));
		}

		private DownloadDialog m_DownloadDialog;
		public DownloadDialog DownloadDialog
		{
			get{return m_DownloadDialog;}
			set{m_DownloadDialog = value;}
		}
	}

	public class TreeNodeDownload : TreeNode
	{
		public TreeNodeDownload(Download p_Download)
		{
			base.Text = p_Download.componentname;

			base.ImageIndex = 6;
			base.SelectedImageIndex = 6;

			m_Download = p_Download;

			base.Tag = p_Download;

			p_Download.ComponentNameChanged += new EventHandler(p_Download_ComponentNameChanged);
		}

		private Download m_Download;
		public Download Download
		{
			get{return m_Download;}
			set{m_Download = value;}
		}

		private void p_Download_ComponentNameChanged(object sender, EventArgs e)
		{
			Text = m_Download.componentname;
		}
	}

	public class TreeNodeComponent : TreeNode
	{
		public TreeNodeComponent(Component p_Component)
		{
			base.Text = p_Component.description;

			if (p_Component is ComponentMsi)
			{
				base.ImageIndex = 11;
				base.SelectedImageIndex = 11;
			}
			else if (p_Component is ComponentCmd)
			{
				base.ImageIndex = 4;
				base.SelectedImageIndex = 4;
			}
			else if (p_Component is ComponentOpenFile)
			{
				base.ImageIndex = 12;
				base.SelectedImageIndex = 12;
			}

			m_Component = p_Component;

			base.Tag = p_Component;

			p_Component.DescriptionChanged +=new EventHandler(p_Component_DescriptionChanged);

			foreach (installedcheck c in p_Component.installchecks)
			{
				Nodes.Add(new TreeNodeinstalledcheck(c));
			}

			if (p_Component.DownloadDialog != null)
				Nodes.Add(new TreeNodeDownloadDialog(p_Component.DownloadDialog));
		}

		private Component m_Component;
		public Component Component
		{
			get{return m_Component;}
			set{m_Component = value;}
		}

		private void p_Component_DescriptionChanged(object sender, EventArgs e)
		{
			Text = m_Component.description;
		}
	}

	public class TreeNodeinstalledcheck : TreeNode
	{
		public TreeNodeinstalledcheck(installedcheck p_installedcheck)
		{
			base.Text = "Installed Check";

			if (p_installedcheck is installedcheck_file)
			{
				base.ImageIndex = 9;
				base.SelectedImageIndex = 9;
			}
			else if (p_installedcheck is installedcheck_registry)
			{
				base.ImageIndex = 10;
				base.SelectedImageIndex = 10;
			}

			base.Tag = p_installedcheck;
		}
	}
}