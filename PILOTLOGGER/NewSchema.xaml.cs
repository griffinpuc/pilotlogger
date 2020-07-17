using AdonisUI;
using System.IO;
using System.Security.Permissions;
using System.Windows;

namespace PILOTLOGGER
{
    /// <summary>
    /// Interaction logic for NewSchema.xaml
    /// </summary>
    public partial class NewSchema : Window
    {

        private string schemaFolderPath;

        public NewSchema(string schemaFolderPath)
        {
            AdonisUI.ResourceLocator.SetColorScheme(Application.Current.Resources, ResourceLocator.DarkColorScheme);

            InitializeComponent();
            this.schemaFolderPath = schemaFolderPath;
        }

        private void addSchema(object sender, RoutedEventArgs e)
        {
            string newSchema = schematext.Text;
            string newSchemaName = newschemaname.Text;
            string[] newValues = newSchema.Split(',');

            if (newValues[newValues.Length-1].Equals(""))
            {
                MessageBox.Show("Invalid schema format!");
            }
            else
            {
                if (newSchemaName.Equals(""))
                {
                    MessageBox.Show("Invalid schema name!");
                }
                else
                {
                    File.WriteAllText(schemaFolderPath + "\\" + newSchemaName + ".schema", newSchema);
                    this.Close();
                }
            }
        }
    }
}
