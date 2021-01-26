using Dicom;
using Dicom.Imaging;
using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace DicomPlayer
{
    public partial class DicomViewerApp : Form
    {
        string[] pathDicomFiles;
        int index;
        string fileName;
        DicomFile dicomFile;
        DicomImage dicomImage;

        public DicomViewerApp()
        {
            BackgroundImage = Properties.Resources.img;
            BackgroundImageLayout = ImageLayout.Stretch;

            InitializeComponent();

            imageDisplay.MouseWheel += 
                new MouseEventHandler(this.imageDisplay_MouseWheel);
        }

        private void imageDisplay_MouseWheel(object sender, MouseEventArgs e)
        {
            if (pathDicomFiles == null)
                return;
            
            if (e.Delta < 0)
                NextFile();
            else
                PrevFile();
        }

        private void ShowDicomImage(string pathFile)
        {
            dicomFile = DicomFile.Open(pathFile);
            dicomImage = new DicomImage(dicomFile.Dataset);
            ImageManager.SetImplementation(WinFormsImageManager.Instance);
            imageDisplay.Image = dicomImage.RenderImage().AsClonedBitmap();
            imageDisplay.SizeMode = PictureBoxSizeMode.Zoom;
            imageDisplay.Refresh(); 
        }

        private void BtnOpenFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                InitialDirectory = ".",
                Filter = "DiCom Image (.dcm;.dic)|*.dcm;*.dic",
                RestoreDirectory = true,
                Multiselect = false
            };

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                if (openFileDialog.FileNames != null)
                {
                    fileName = openFileDialog.FileName;
                    pathDicomFiles = Directory.GetFiles(Path.GetDirectoryName(fileName));

                    index = pathDicomFiles.ToList().IndexOf(fileName);
                    ShowDicomImage(pathDicomFiles[index]);
                }
            }
        }

        private void NextFile()
        {
            if (++index >= pathDicomFiles.Length)
                index = 0;

            ShowDicomImage(pathDicomFiles[index]);
        }

        private void PrevFile()
        {
            if (--index < 0)
                index = pathDicomFiles.Length - 1;

            ShowDicomImage(pathDicomFiles[index]);
        }

        private void DicomViewerApp_KeyPress(object sender, KeyPressEventArgs e)
        {
            MessageBox.Show("");
        }
    }
}