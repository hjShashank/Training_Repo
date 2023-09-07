/*********************************************************************************
**                   AVIN SYSTEMS PVT LTD                                       **
**********************************************************************************
** Copyright (C) AVIN SYSTEMS PVT LTD 2021 - All Rights Reserved                **
** Reproduction, distribution and utilization of this document as well          **
** as the communication of its contents to others without explicit              **
** authorization is prohibited.                                                 **
** Offenders will be held liable for the payment of damages.                    **
**********************************************************************************
**  File Name   : GEN_StartPage_View.xaml.cs						            **
**                                                                              **
**  PURPOSE     : Place holder for recent files and tool basic functionalities 	**
**				  which helps in understanding creating/opening projects.		**
**				  																**
**                                                                              **
**********************************************************************************/

/*********************************************************************************
** Change History                                                               **
**********************************************************************************
 
 Revision: 1.0.0     |Date: 13-Jul-2021          |Author: Hardhik Yadav Isarapu 
 - Initial version
  
 Revision: 1.1.0     |Date: 12-Aug-2021          |Author: Rakesh Jadhav
 -As per CR 11224 code file is updated for bug fixing

 Revision: 2.0.0     |Date: 22-Oct-2021          |Author: Rakesh Jadhav
 -As per CR 11358 code file is updated for bug fixing
**********************************************************************************/
using AVIN_FS_Tool.Views;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace AVIN_FS_Tool
{
    /// <summary>
    /// Class Name         : GEN_StartPage_View
    /// Class Description  : Interaction logic for GEN_StartPage_View.xaml
    /// </summary>
    public partial class GEN_StartPage_View : UserControl
    {

        #region Fileds
        public class User
        {
            public string FileName { get; set; }

            public string FilePath { get; set; }

        }
        public GEN_StartPage_ViewModel startPageView;
        #endregion

        #region Default Constructor

        /// <summary>
        /// Constructor Name         : GEN_StartPage_View
        /// Constructor Description  : initialization of controls in StartPage.xaml(wpfstartpage) and Loading of recent files
        /// </summary>
        public GEN_StartPage_View()
        {
            InitializeComponent();
            startPageView = new GEN_StartPage_ViewModel();

        } // End of public StartPage() 
        #endregion


        #region Create Existing Project

        /// <summary>
        /// Method Name              : Existing_Project
        /// Method Description       : -
        /// Requirement ID           : - 
        /// Method input Parameters  : object sender, RoutedEventArgs e
        /// Param Name	             : -
        /// Methods Return Parameter : void
        /// </summary>
        private void Existing_Project(object sender, RoutedEventArgs e)
        {
            GlobalClass.Glob_MainWindow.OpenExistingProject();
        } // End of private void Hyperlink_click(object sender, RoutedEventArgs e)   
        #endregion

        #region Open New Project

        /// <summary>
        /// Method Name              : New_Project
        /// Method Description       : -
        /// Requirement ID           : - 
        /// Method input Parameters  : object sender, RoutedEventArgs e
        /// Param Name	             : -
        /// Methods Return Parameter : void
        /// </summary>
        private void New_Project(object sender, RoutedEventArgs e)
        {

            GlobalClass.Glob_MainWindow.NewProject();
        } // End of private void Hyperlink_click1(object sender, RoutedEventArgs e)

        #endregion

        #region Listbox_MouseDouble Click

        /// <summary>
        /// Method Name              : listbox_MouseDoubleClick
        /// Method Description       : Selecting a specified file from the list of recent files
        /// Requirement ID           : - 
        /// Method input Parameters  : object sender, MouseButtonEventArgs e
        /// Param Name	             : -
        /// Methods Return Parameter : void
        /// </summary>
        private void listbox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if((listbox.SelectedItem as GEN_StartPage_View.User).FileName.Equals(string.Empty))
                {
                    return;
                }
                var i = listbox.SelectedIndex;

                if (i.Equals(-1))
                {
                    return;
                }

                if (GlobalClass.Glob_instance_fs.MWndVM_ProjectCollection.Count != 0)
                {
                    if (GlobalClass.Glob_instance_fs.MWndVM_ProjectCollection[0].
                    PrjVM_Header != "TCL_Sample_Project"
                    && GlobalClass.Glob_instance_fs.MWndVM_ProjectCollection[0].
                    PrjVM_Header != "HARA_Sample_Project"
                    && GlobalClass.Glob_instance_fs.MWndVM_ProjectCollection[0].
                    PrjVM_Header != "SAR_Sample_Project")
                    {

                        Form_Question_Dialogue fqd = new Form_Question_Dialogue(
                        "Do you want to save the current working project before opening new project?");
                        System.Windows.Forms.DialogResult DR = fqd.ShowDialog();
                        if (DR.Equals(System.Windows.Forms.DialogResult.Yes))
                        {
                            Mouse.SetCursor(Cursors.Wait);
                            SaveEntireProject Save = new SaveEntireProject();
                            GlobalClass.Glob_Error = Save.SaveAllTools();
                            if (GlobalClass.Glob_Error)
                            {
                                return;
                            }// End of if (GlobalClass.Glob_Error)
                            GEN_BackEnd back_end = new GEN_BackEnd();
                            back_end.BckEnd_SerializeAllData(GlobalClass.
                            Glob_instance_fs.MWndVM_ProjectCollection[0]);
                            var Tab = GlobalClass.Glob_MainWindow.tab1.Items.
                            OfType<TabControlItemHeader.ActionTabItem>().
                            Where(TabItem => TabItem.Header.Equals("Diagram")).FirstOrDefault();

                            if (Tab != null)
                            {
                                GlobalClass.Glob_MainWindow.SaveUsingEncoder(
                                "ItemDefinitionDiagram.png", (Tab.Content as ItemDefinition).
                                canvasArea, new PngBitmapEncoder(), null);
                            }
                        }
                        else
                        {
                            Mouse.SetCursor(Cursors.Wait);
                        }
                    }
                }

                GlobalClass.Glob_path = filePath[i];
                if (string.IsNullOrEmpty(GlobalClass.Glob_path))
                {
                    return;
                }
                else if (!File.Exists(GlobalClass.Glob_path))
                {
                    Form_Error_Dialogue frr =
                    new Form_Error_Dialogue("File not found.",
                    StringConstants.StrCons_ERRORMSG);
                    frr.ShowDialog();
                    // fileName = fileName.Where(w => w != fileName[i]).ToArray();
                    // foreach(var v in fileName)
                    // {
                    //    listbox.Items.Add(v);
                    // }
                    return;
                }
                GlobalClass.Glob_filename_without_extension = fileName[i];

                GlobalClass.Glob_Lines = File.ReadAllLines(GlobalClass.Glob_path);
                // Check if selected file is empty
                GlobalClass.Glob_MainWindow.IsEmptyorOpen(GlobalClass.Glob_Lines,
                GlobalClass.Glob_filename_without_extension);

                if (i == 0)
                    return;

                Helper_RecentFilesHistory rFH = new Helper_RecentFilesHistory();
                rFH.RecentFileHistorySort(true, i, null, null);
                // rFH.SortOpenExistingProject(i);
                Mouse.SetCursor(Cursors.Arrow);
            }
            catch
            {
                Mouse.SetCursor(Cursors.Arrow);
                Form_Error_Dialogue frr = new Form_Error_Dialogue("File not found.",
                StringConstants.StrCons_ERRORMSG);
                frr.ShowDialog();
            }
        } // End of private void listbox_MouseDoubleClick(object sender, MouseButtonEventArgs e) 


        #endregion

        #region Recent File History
        static readonly string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;

        static readonly string textFile = string.Concat(baseDirectory,
        "RecentFileHisotry.txt");
        public string[] fileName = new string[4];
        public string[] filePath = new string[4];

        /// <summary>
        /// Method Name              : Listbox_Loaded
        /// Method Description       : -
        /// Requirement ID           : - 
        /// Method input Parameters  : object sender, RoutedEventArgs e
        /// Param Name	             : -
        /// Methods Return Parameter : void
        /// </summary>
        private void Listbox_Loaded(object sender, RoutedEventArgs e)
        {
            RecentHisotryFileExists();
            if (File.Exists(textFile))
            {

                // Read a text file line by line.
                string[] lines = File.ReadAllLines(textFile);
                Helper_RecentFilesHistory rFH = new Helper_RecentFilesHistory();

                fileName = rFH.readTextFile(lines, 0, 4);
                filePath = rFH.readTextFile(lines, 5, 9);

                List<User> items = new List<User>();
                for (int l = 0; l < 4; l++)
                {
                    items.Add(new User() { FileName = fileName[l], FilePath = filePath[l] });
                    // items.Add(new User() { FileName = "Item1_HARA", FilePath = "c:\\file1.txt" });
                    // items.Add(new User() { FileName = "AVIN_SAR_AUTOSAR", FilePath = "c:\\file1.txt" });
                }
                listbox.ItemsSource = items;
            }

        }

        /// <summary>
        /// Method Name              : RecentHisotryFileExists
        /// Method Description       : -
        /// Requirement ID           : - 
        /// Method input Parameters  : -
        /// Param Name	             : -
        /// Methods Return Parameter : void
        /// </summary>
        private void RecentHisotryFileExists()
        {

            // var baseDirectory = AppDomain.CurrentDomain.BaseDirectory;

            var mFilepath = string.Concat(baseDirectory, "RecentFileHisotry.txt");

            if (File.Exists(mFilepath))
            {
                return;
            }

            using (StreamWriter sw = File.CreateText(mFilepath))
            {
                sw.WriteLine("");
                sw.WriteLine("");
                sw.WriteLine("");
                sw.WriteLine("");
                sw.WriteLine("");
                sw.WriteLine("");
                sw.WriteLine("");
                sw.WriteLine("");
                sw.WriteLine("");
                sw.WriteLine("");
            }
        }

        #endregion


        /// <summary>
        /// Method Name              : Hyperlink_Click
        /// Method Description       : -
        /// Requirement ID           : - 
        /// Method input Parameters  : object sender, RoutedEventArgs e
        /// Param Name	             : -
        /// Methods Return Parameter : void
        /// </summary>
        private void Hyperlink_Click(object sender, RoutedEventArgs e)
        {
            Form_WhatsNewForm f1 = new Form_WhatsNewForm();
            f1.ShowDialog();
        }

        /// <summary>
        /// Method Name              : AboutTool
        /// Method Description       : -
        /// Requirement ID           : - 
        /// Method input Parameters  : object sender, RoutedEventArgs e
        /// Param Name	             : -
        /// Methods Return Parameter : void
        /// </summary>
        private void AboutTool(object sender, RoutedEventArgs e)
        {

            Form_AboutTool aboutTool = new Form_AboutTool();
            aboutTool.ShowDialog();
        }


        /// <summary>
        /// Method Name              : Button_Click
        /// Method Description       : -
        /// Requirement ID           : - 
        /// Method input Parameters  : object sender, RoutedEventArgs e
        /// Param Name	             : -
        /// Methods Return Parameter : void
        /// </summary>
        private void Button_Click(object sender, RoutedEventArgs e)
        {

            GEN_Abbreviation_ViewModel gAbbViewModel = GlobalClass.Glob_MainWindow.
            GetInstanceofgAbbViewModel();
            GEN_Abbreviations ab = new GEN_Abbreviations(gAbbViewModel);

        }

        /// <summary>
        /// Method Name              : Button_Click_1
        /// Method Description       : -
        /// Requirement ID           : - 
        /// Method input Parameters  : object sender, RoutedEventArgs e
        /// Param Name	             : -
        /// Methods Return Parameter : void
        /// </summary>
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            DirectoryInfo D = Licence.GetExeDirectory();

            if (D == null)
            {
                return;
            }
            FileInfo tclfilename = Licence.GetFile("TCL_Sample_Project.afs", "*.afs");
            if (fileName == null)
            {
                return;
            }
            GlobalClass.Glob_NewProject = new OpenNewProject();

            // Checking for project in FunctionalsafetyViewModel 
            if (GlobalClass.Glob_instance_fs.MWndVM_ProjectCollection.Count != 0)
            {
                if (GlobalClass.Glob_instance_fs.MWndVM_ProjectCollection[0].
				PrjVM_Header != "TCL_Sample_Project"
                && GlobalClass.Glob_instance_fs.MWndVM_ProjectCollection[0].
				PrjVM_Header != "HARA_Sample_Project"
                && GlobalClass.Glob_instance_fs.MWndVM_ProjectCollection[0].
				PrjVM_Header != "SAR_Sample_Project")
                {
                    // Prompt Message for Saving the Current Project
                    // System.Windows.Forms.DialogResult dr = System.Windows.Forms.MessageBox.Show("Do you want to save the current Project?", "Save Project", System.Windows.Forms.MessageBoxButtons.YesNo, System.Windows.Forms.MessageBoxIcon.Question);
                    Form_Question_Dialogue fqd = new Form_Question_Dialogue(
                    "Do you want to save the current Project?");
                    System.Windows.Forms.DialogResult dr = fqd.ShowDialog();
                    // If yes save and serialize data
                    if (dr.Equals(System.Windows.Forms.DialogResult.Yes))
                    {
                        SaveEntireProject Save = new SaveEntireProject();
                        // Checking for error occured during saving the project
                        GlobalClass.Glob_Error = Save.SaveAllTools();
                        // if yes return
                        if (GlobalClass.Glob_Error)
                        {
                            return;
                        }// End of if (GlobalClass.Glob_Error)
                        GEN_BackEnd back_end = new GEN_BackEnd();
                        back_end.BckEnd_SerializeAllData(GlobalClass.
                        Glob_instance_fs.MWndVM_ProjectCollection[0]);
                    } // End of if (dr == System.Windows.Forms.DialogResult.Yes)

                }
            }


            Mouse.SetCursor(System.Windows.Input.Cursors.Wait);
            try
            {

                GlobalClass.Glob_path = Path.GetFullPath(tclfilename.Name);

                GlobalClass.Glob_filename = tclfilename.Name;
                // Get the selected file name without extension from opendialogbox
                GlobalClass.Glob_filename_without_extension = "TCL_Sample_Project";
                // Read all the content line by line from selected file path from opendialogbox and return value is assigned to string array
                GlobalClass.Glob_Lines = File.ReadAllLines(GlobalClass.Glob_path);

                // Check if selected file is empty
                IsEmptyorOpen(GlobalClass.Glob_Lines, GlobalClass.Glob_filename_without_extension);

                TabsRemove();
            }
            catch
            {
            }
            // Change mouse cursor to arrow from default cursors
            Mouse.SetCursor(System.Windows.Input.Cursors.Arrow);
        }

        /// <summary>
        /// Method Name              : Button_Click_2
        /// Method Description       : -
        /// Requirement ID           : - 
        /// Method input Parameters  : object sender, RoutedEventArgs e
        /// Param Name	             : -
        /// Methods Return Parameter : void
        /// </summary>
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            DirectoryInfo D = Licence.GetExeDirectory();

            if (D == null)
            {
                return;
            }
            FileInfo harafilename = Licence.GetFile("HARA_Sample_Project.afs", "*.afs");
            if (fileName == null)
            {
                return;
            }
            GlobalClass.Glob_NewProject = new OpenNewProject();

            // Checking for project in FunctionalsafetyViewModel 
            if (GlobalClass.Glob_instance_fs.MWndVM_ProjectCollection.Count != 0)
            {
                if (GlobalClass.Glob_instance_fs.MWndVM_ProjectCollection[0].
				PrjVM_Header != "TCL_Sample_Project"
                && GlobalClass.Glob_instance_fs.MWndVM_ProjectCollection[0].
				PrjVM_Header != "HARA_Sample_Project"
                && GlobalClass.Glob_instance_fs.MWndVM_ProjectCollection[0].
				PrjVM_Header != "SAR_Sample_Project")
                {
                    Form_Question_Dialogue fqd = new Form_Question_Dialogue(
                    "Do you want to save the current Project?");
                    System.Windows.Forms.DialogResult dr = fqd.ShowDialog();
                    // If yes save and serialize data
                    if (dr.Equals(System.Windows.Forms.DialogResult.Yes))
                    {
                        SaveEntireProject Save = new SaveEntireProject();
                        // Checking for error occured during saving the project
                        GlobalClass.Glob_Error = Save.SaveAllTools();
                        // if yes return
                        if (GlobalClass.Glob_Error)
                        {
                            return;
                        }// End of if (GlobalClass.Glob_Error)
                        GEN_BackEnd back_end = new GEN_BackEnd();
                        back_end.BckEnd_SerializeAllData(
                        GlobalClass.Glob_instance_fs.MWndVM_ProjectCollection[0]);
                    } // End of if (dr == System.Windows.Forms.DialogResult.Yes)                   
                }
            }

            Mouse.SetCursor(System.Windows.Input.Cursors.Wait);
            try
            {
                // Get the path of the selected file from opendialogbox
                GlobalClass.Glob_path = Path.GetFullPath(harafilename.Name);

                GlobalClass.Glob_filename = harafilename.Name;
                // Get the selected file name without extension from opendialogbox
                GlobalClass.Glob_filename_without_extension = "HARA_Sample_Project";
                // Read all the content line by line from selected file path from opendialogbox and return value is assigned to string array
                GlobalClass.Glob_Lines = File.ReadAllLines(GlobalClass.Glob_path);

                // Check if selected file is empty
                IsEmptyorOpen(GlobalClass.Glob_Lines, GlobalClass.Glob_filename_without_extension);

                TabsRemove();

                GlobalClass.Glob_MainWindow.GENMW_ToolKitClick();
                GlobalClass.Glob_MainWindow.GENMW_PropertiesClick();
                GlobalClass.Glob_MainWindow.CheckForTab("Project Browser");
            }
            catch
            {

            }
            // Change mouse cursor to arrow from default cursors
            Mouse.SetCursor(System.Windows.Input.Cursors.Arrow);

        }

        /// <summary>
        /// Method Name              : TabsRemove
        /// Method Description       : -
        /// Requirement ID           : - 
        /// Method input Parameters  : -
        /// Param Name	             : -
        /// Methods Return Parameter : void
        /// </summary>
        public void TabsRemove()
        {
            for (int i = GlobalClass.Glob_MainWindow.CBV_DynamicSource.
            RightTabControl.Items.Count - 1; i >= 0; i--)
            {
                if ((GlobalClass.Glob_MainWindow.CBV_DynamicSource.
                RightTabControl.Items[i] as TabControlItemHeader.
                ActionTabItem).Header == "Properties" ||
                (GlobalClass.Glob_MainWindow.CBV_DynamicSource.
                RightTabControl.Items[i] as TabControlItemHeader.
                ActionTabItem).Header == "Tool Kit")
                {
                    GlobalClass.Glob_MainWindow.CBV_DynamicSource.
                    RightTabControl.Items.Remove(GlobalClass.Glob_MainWindow.
                    CBV_DynamicSource.RightTabControl.Items[i]);
                    GlobalClass.Glob_MainWindow.ToolKit.IsChecked = false;
                    GlobalClass.Glob_MainWindow.Properties.IsChecked = false;
                }
            }

        }

        /// <summary>
        /// Method Name              : IsEmptyorOpen
        /// Method Description       : -
        /// Requirement ID           : - 
        /// Method input Parameters  : string[] lines, string filename
        /// Param Name	             : -
        /// Methods Return Parameter : void
        /// </summary>
        private void IsEmptyorOpen(string[] lines, string filename)
        {
            if (!(string.IsNullOrEmpty(filename)))
            {
                // Check length of the array
                if (lines.Length == 0)
                {
                    // Display the warning if array is empty
                    System.Windows.Forms.MessageBox.Show(
                    "File format is not supported", "Warning",
                    System.Windows.Forms.MessageBoxButtons.OK,
                    System.Windows.Forms.MessageBoxIcon.Warning);
                } // End of if (lines.Length == 0)
                else
                {
                    try
                    {
                        GlobalClass.Glob_MainWindow.ClearDataContext();
                        // Instantiate a new instance for decryption class
                        GEN_Decryption decrypt_data = new GEN_Decryption();
                        // Get the decrypted .afs file into project structure(Collection)
                        GEN_Project_ViewModel pro_view_model = decrypt_data.Decryp_Decrypter();
                        // Check for the project structure(collection) is null
                        if (pro_view_model != null)
                        {
                            GlobalClass.Glob_instance_fs.BaseVM_Columns =
                            pro_view_model.BaseVM_Columns;
                            // Set the tooltile with project name
                            GlobalClass.Glob_MainWindow.Title =
                            StringConstants.StrCons_ToolTitle + " - " + filename;

                            if (GlobalClass.Glob_instance_fs.MWndVM_ProjectCollection.
                            Count.Equals(GlobalClass.Glob_ZeroIndex))
                            {
                                GlobalClass.Glob_instance_fs.
                                MWndVM_ProjectCollection.Add(pro_view_model);
                            }
                            else
                            {
                                GlobalClass.Glob_instance_fs.MWndVM_ProjectCollection.Clear();
                                GlobalClass.Glob_instance_fs.
                                MWndVM_ProjectCollection.Add(pro_view_model);
                            }

                            // Enable save, saveall, import, export, closeproject properties by passing input parameter as true
                            GlobalClass.Glob_MainWindow.EnableDisableOnClose(true);
                        } // End of if (pro_view_model != null)
                        else
                        {
                            // Display the warning if projectviewmodel observablecollection is null
                            System.Windows.Forms.MessageBox.Show("File format is not supported",
                            "Warning", System.Windows.Forms.MessageBoxButtons.OK,
                            System.Windows.Forms.MessageBoxIcon.Warning);
                        }// End of else
                    }
                    catch
                    {
                    }
                } // End of else
            }// End 
        }

        /// <summary>
        /// Method Name              : TechSupport_MouseLeftButtonDown
        /// Method Description       : -
        /// Requirement ID           : - 
        /// Method input Parameters  : object sender, MouseButtonEventArgs e
        /// Param Name	             : -
        /// Methods Return Parameter : void
        /// </summary>
        private void TechSupport_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            GlobalClass.Glob_MainWindow.TechSupport();
        }

        /// <summary>
        /// Method Name              : TextBlock_Loaded
        /// Method Description       : -
        /// Requirement ID           : - 
        /// Method input Parameters  : object sender, RoutedEventArgs e
        /// Param Name	             : -
        /// Methods Return Parameter : void
        /// </summary>
        private void TextBlock_Loaded(object sender, RoutedEventArgs e)
        {
            (sender as TextBlock).Text = "4th & 5th Floor, Enzyme Tech Park #142, Hosur Rd,";
        }

        /// <summary>
        /// Method Name              : TextBlock_Loaded_1
        /// Method Description       : -
        /// Requirement ID           : - 
        /// Method input Parameters  : object sender, RoutedEventArgs e
        /// Param Name	             : -
        /// Methods Return Parameter : void
        /// </summary>
        private void TextBlock_Loaded_1(object sender, RoutedEventArgs e)
        {
            (sender as TextBlock).Text =
            "Koramangala Industrial Layout, Bengaluru, Karnataka 560095.";
        }

        /// <summary>
        /// Method Name              : EmailLoaded
        /// Method Description       : -
        /// Requirement ID           : - 
        /// Method input Parameters  : object sender, RoutedEventArgs e
        /// Param Name	             : -
        /// Methods Return Parameter : void
        /// </summary>
        private void EmailLoaded(object sender, RoutedEventArgs e)
        {
            (sender as System.Windows.Documents.Run).Text =
            "avinfstool.support@avinsystems.com";
        }


        /// <summary>
        /// Method Name              : Help_click
        /// Method Description       : -
        /// Requirement ID           : - 
        /// Method input Parameters  : object sender, RoutedEventArgs e
        /// Param Name	             : -
        /// Methods Return Parameter : void
        /// </summary>
        private void Help_click(object sender, RoutedEventArgs e)
        {
            string assemblypath = System.Reflection.Assembly.GetExecutingAssembly().Location;
            string directionname = System.IO.Path.GetDirectoryName(assemblypath);
            string helpfilepath = System.IO.Path.Combine(directionname, "AVIN_FSATOOL_UM.chm");

            if (File.Exists(helpfilepath))
            {
                System.Diagnostics.Process.Start(helpfilepath); ;
            }
        }
        /// <summary>
        /// Method Name              : Button_Click_3
        /// Method Description       : -
		/// Requirement ID           : - 
        /// Method input Parameters  : object sender, RoutedEventArgs e
		/// Param Name	             : -
        /// Methods Return Parameter : void
        /// </summary>
        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            DirectoryInfo D = Licence.GetExeDirectory();

            if (D == null)
            {
                return;
            }
            FileInfo tclfilename = Licence.GetFile("SAR_Sample_Project.afs", "*.afs");
            if (fileName == null)
            {
                return;
            }
            GlobalClass.Glob_NewProject = new OpenNewProject();

            // Checking for project in FunctionalsafetyViewModel 
            if (GlobalClass.Glob_instance_fs.MWndVM_ProjectCollection.Count != 0)
            {
                if (GlobalClass.Glob_instance_fs.MWndVM_ProjectCollection[0].
				PrjVM_Header != "TCL_Sample_Project" 
                && GlobalClass.Glob_instance_fs.MWndVM_ProjectCollection[0].
				PrjVM_Header != "HARA_Sample_Project"
                && GlobalClass.Glob_instance_fs.MWndVM_ProjectCollection[0].
				PrjVM_Header != "SAR_Sample_Project")
                {
                    // Prompt Message for Saving the Current Project
                    // System.Windows.Forms.DialogResult dr = System.Windows.Forms.MessageBox.Show("Do you want to save the current Project?", "Save Project", System.Windows.Forms.MessageBoxButtons.YesNo, System.Windows.Forms.MessageBoxIcon.Question);
                    Form_Question_Dialogue fqd = new Form_Question_Dialogue(
                    "Do you want to save the current Project?");
                    System.Windows.Forms.DialogResult dr = fqd.ShowDialog();
                    // If yes save and serialize data
                    if (dr.Equals(System.Windows.Forms.DialogResult.Yes))
                    {
                        SaveEntireProject Save = new SaveEntireProject();
                        // Checking for error occured during saving the project
                        GlobalClass.Glob_Error = Save.SaveAllTools();
                        // if yes return
                        if (GlobalClass.Glob_Error)
                        {
                            return;
                        }// End of if (GlobalClass.Glob_Error)
                        GEN_BackEnd back_end = new GEN_BackEnd();
                        back_end.BckEnd_SerializeAllData(GlobalClass.
                        Glob_instance_fs.MWndVM_ProjectCollection[0]);
                    } // End of if (dr == System.Windows.Forms.DialogResult.Yes)

                }
            }


            Mouse.SetCursor(System.Windows.Input.Cursors.Wait);
            try
            {
                // Get the path of the selected file from opendialogbox
                GlobalClass.Glob_path = Path.GetFullPath(tclfilename.Name);


                // Get the selected file name from opendialogbox
                GlobalClass.Glob_filename = tclfilename.Name;
                // Get the selected file name without extension from opendialogbox
                GlobalClass.Glob_filename_without_extension = "SAR_Sample_Project";
                // Read all the content line by line from selected file path from opendialogbox and return value is assigned to string array
                GlobalClass.Glob_Lines = File.ReadAllLines(GlobalClass.Glob_path);

                // Check if selected file is empty
                IsEmptyorOpen(GlobalClass.Glob_Lines, GlobalClass.Glob_filename_without_extension);
                TabsRemove();

                GlobalClass.Glob_MainWindow.GENMW_ToolKitClick();
                GlobalClass.Glob_MainWindow.CheckForTab("Project Browser");
            }
            catch
            {
            }
            // Change mouse cursor to arrow from default cursors
            Mouse.SetCursor(System.Windows.Input.Cursors.Arrow);
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            DirectoryInfo D = Licence.GetExeDirectory();

            if (D == null)
            {
                return;
            }
            FileInfo tclfilename = Licence.GetFile("SC_Sample_Project.afs", "*.afs");
            if (fileName == null)
            {
                return;
            }
            GlobalClass.Glob_NewProject = new OpenNewProject();

            // Checking for project in FunctionalsafetyViewModel 
            if (GlobalClass.Glob_instance_fs.MWndVM_ProjectCollection.Count != 0)
            {
                if (GlobalClass.Glob_instance_fs.MWndVM_ProjectCollection[0].
                PrjVM_Header != "TCL_Sample_Project"
                && GlobalClass.Glob_instance_fs.MWndVM_ProjectCollection[0].
                PrjVM_Header != "HARA_Sample_Project"
                && GlobalClass.Glob_instance_fs.MWndVM_ProjectCollection[0].
                PrjVM_Header != "SAR_Sample_Project")
                {
                    // Prompt Message for Saving the Current Project
                    // System.Windows.Forms.DialogResult dr = System.Windows.Forms.MessageBox.Show("Do you want to save the current Project?", "Save Project", System.Windows.Forms.MessageBoxButtons.YesNo, System.Windows.Forms.MessageBoxIcon.Question);
                    Form_Question_Dialogue fqd = new Form_Question_Dialogue(
                    "Do you want to save the current Project?");
                    System.Windows.Forms.DialogResult dr = fqd.ShowDialog();
                    // If yes save and serialize data
                    if (dr.Equals(System.Windows.Forms.DialogResult.Yes))
                    {
                        SaveEntireProject Save = new SaveEntireProject();
                        // Checking for error occured during saving the project
                        GlobalClass.Glob_Error = Save.SaveAllTools();
                        // if yes return
                        if (GlobalClass.Glob_Error)
                        {
                            return;
                        }// End of if (GlobalClass.Glob_Error)
                        GEN_BackEnd back_end = new GEN_BackEnd();
                        back_end.BckEnd_SerializeAllData(GlobalClass.
                        Glob_instance_fs.MWndVM_ProjectCollection[0]);
                    } // End of if (dr == System.Windows.Forms.DialogResult.Yes)

                }
            }


            Mouse.SetCursor(System.Windows.Input.Cursors.Wait);
            try
            {

                GlobalClass.Glob_path = Path.GetFullPath(tclfilename.Name);

                GlobalClass.Glob_filename = tclfilename.Name;
                // Get the selected file name without extension from opendialogbox
                GlobalClass.Glob_filename_without_extension = "SC_Sample_Project";
                // Read all the content line by line from selected file path from opendialogbox and return value is assigned to string array
                GlobalClass.Glob_Lines = File.ReadAllLines(GlobalClass.Glob_path);

                // Check if selected file is empty
                IsEmptyorOpen(GlobalClass.Glob_Lines, GlobalClass.Glob_filename_without_extension);

                TabsRemove();
            }
            catch
            {
            }
            // Change mouse cursor to arrow from default cursors
            Mouse.SetCursor(System.Windows.Input.Cursors.Arrow);
        }
    } // End of public partial class StartPage : UserControl
} // End of namespace AVIN_FS_Tool

/**********************************************************************************
**                           End of File                                         **
***********************************************************************************/
