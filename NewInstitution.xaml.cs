using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using WpfMedaSlovenija;

namespace WpfMedaSlovenija
{
    public partial class NewInstitution : Window
    {
        #region Properties
        public vwInstitution objInstitution { get; set; }
        private MedaEntitiesDb mdb = new MedaEntitiesDb();
        public int index;
        public bool result;
        #endregion

        #region Constructor
        public NewInstitution()
        {
            InitializeComponent();
            if (objInstitution == null)
                objInstitution = new vwInstitution();
        }
        #endregion

        #region Enum
        enum TableName
        {
            vwInstitution,
            tblInstitution,
            Headoffice
        }
        #endregion

        #region Methods
        private void SetCities()
        {
            int ctrID = objInstitution.ctrID;
            List<tblCity> listCity = (from c in mdb.tblCities
                                      join d in mdb.tblDistricts on c.dstID equals d.dstID
                                      where d.ctrID == ctrID
                                      orderby c.ctyName
                                      select c).ToList<tblCity>();
            cmbCity.ItemsSource = listCity;
            cmbCity.DisplayMemberPath = "ctyName";
            cmbCity.SelectedValuePath = "ctyID";
        }

        private void SetSubTypes()
        {
            List<tblInstitutionSubtype> listSubTypes = mdb.tblInstitutionSubtypes.OrderBy(n => n.itpID)
                .ToList();
            cmbSubType.ItemsSource = listSubTypes;
            cmbSubType.DisplayMemberPath = "isbName";
            cmbSubType.SelectedValuePath = "isbID";
        }

        private void SetHeadoffices()
        {

            int ctr = objInstitution.ctrID;
            switch (ctr)
            {
                case 1:
                    dgHeadoffice.ItemsSource = mdb.tblHeadofficeSerbias.Where(n => n.staID == 1).OrderBy(n => n.hdoName).ToList();
                    break;
                case 3:
                    dgHeadoffice.ItemsSource = mdb.tblHeadofficeCroatias.Where(n => n.staID == 1).OrderBy(n => n.hdoName).ToList();
                    break;
                case 4:
                    dgHeadoffice.ItemsSource = mdb.tblHeadofficeSlovenias.Where(n => n.staID == 1).OrderBy(n => n.hdoName).ToList();
                    break;
                default:
                    break;
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            SetCities();
            SetSubTypes();
            SetHeadoffices();
            if (objInstitution != null)
            {
                txtID.Text = (objInstitution.insID==0)?null:objInstitution.insID.ToString();
                txtName.Text = objInstitution.insName;
                txtAddress.Text = objInstitution.insAddress;
                cmbCity.SelectedValue = objInstitution.ctyID;
                cmbSubType.SelectedValue = objInstitution.isbID;
                txtHeadofficeID.Text = objInstitution.hdoID.ToString();
                txtHeadofficeName.Text = objInstitution.hdoName;
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!ValidateFields()) return;
                object _newInstitution = Activator.CreateInstance(Type.GetType(GetClassName(objInstitution.ctrID, TableName.tblInstitution)));
                PropertyInfo[] propertiesInfo = _newInstitution.GetType().GetProperties();
                foreach (PropertyInfo pInfo in propertiesInfo)
                {
                    switch (pInfo.Name)
                    {
                        case "insID":
                            if (!String.IsNullOrWhiteSpace(txtID.Text)) pInfo.SetValue(_newInstitution, Convert.ChangeType(Int32.Parse(txtID.Text), pInfo.PropertyType), null);
                            break;
                        case "insName":
                            pInfo.SetValue(_newInstitution, Convert.ChangeType(txtName.Text, pInfo.PropertyType), null);
                            break;
                        case "insAddress":
                            pInfo.SetValue(_newInstitution, Convert.ChangeType(txtAddress.Text, pInfo.PropertyType), null);
                            break;
                        case "ctyID":
                            pInfo.SetValue(_newInstitution, Convert.ChangeType(Int32.Parse(cmbCity.SelectedValue.ToString()), pInfo.PropertyType), null);
                            break;
                        case "isbID":
                            pInfo.SetValue(_newInstitution, Convert.ChangeType(Int32.Parse(cmbSubType.SelectedValue.ToString()), pInfo.PropertyType), null);
                            break;
                        case "hdoID":
                            pInfo.SetValue(_newInstitution, Convert.ChangeType(Int32.Parse(txtHeadofficeID.Text), pInfo.PropertyType), null);
                            break;
                        default:
                            break;
                    }
                }

                index = DataAccessLayer.SaveInstitution(_newInstitution, objInstitution.ctrID);
                result = true;
                if (index > 0)
                    Close();
                else
                    result = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                result = false;
            } 
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            result = false;
            this.Close();
        }

        private void txtSearchHeadoffice_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!String.IsNullOrWhiteSpace(txtSearchHeadoffice.Text))
            {
                dgHeadoffice.ItemsSource = null;
                switch (objInstitution.ctrID)
                {
                    case 1:
                        dgHeadoffice.ItemsSource = mdb.tblHeadofficeSerbias.Where(n => n.hdoName.Contains(txtSearchHeadoffice.Text)).Where(n => n.staID == 1).ToList();
                        break;
                    case 3:
                        dgHeadoffice.ItemsSource = mdb.tblHeadofficeCroatias.Where(n => n.hdoName.Contains(txtSearchHeadoffice.Text)).Where(n=>n.staID==1).ToList();
                        break;
                    case 4:
                        dgHeadoffice.ItemsSource = mdb.tblHeadofficeSlovenias.Where(n => n.hdoName.Contains(txtSearchHeadoffice.Text)).Where(n => n.staID == 1).ToList();
                        break;
                    default:
                        break;
                }
            }
            else
            {
                dgHeadoffice.ItemsSource = null;
                switch (objInstitution.ctrID)
                {
                    case 1:
                        dgHeadoffice.ItemsSource = mdb.tblHeadofficeSerbias.Where(n => n.staID == 1).ToList();
                        break;
                    case 3:
                        dgHeadoffice.ItemsSource = mdb.tblHeadofficeCroatias.Where(n => n.staID == 1).ToList();
                        break;
                    case 4:
                        dgHeadoffice.ItemsSource = mdb.tblHeadofficeSlovenias.Where(n => n.staID == 1).ToList();
                        break;
                    default:
                        break;
                }
            }
        }
        private string GetClassName(int ctrID, TableName _tableName)
        {
            switch (ctrID)
            {
                case 1:
                    switch (_tableName)
                    {
                        case TableName.tblInstitution:
                            return "WpfMedaSlovenija.tblInstitutionSerbia";
                        case TableName.vwInstitution:
                            return "WpfMedaSlovenija.vwInstitutionSerbia";
                        case TableName.Headoffice:
                            return "WpfMedaSlovenija.tblHeadofficeSerbia";
                        default:
                            return null;
                    }
                case 3:
                    switch (_tableName)
                    {
                        case TableName.tblInstitution:
                            return "WpfMedaSlovenija.tblInstitutionCroatia";
                        case TableName.vwInstitution:
                            return "WpfMedaSlovenija.vwInstitutionCroatia";
                        case TableName.Headoffice:
                            return "WpfMedaSlovenija.tblHeadofficeCroatia";
                        default:
                            return null;
                    }
                case 4:
                    switch (_tableName)
                    {
                        case TableName.tblInstitution:
                            return "WpfMedaSlovenija.tblInstitutionSlovenia";
                        case TableName.vwInstitution:
                            return "WpfMedaSlovenija.vwInstitutionSlovenia";
                        case TableName.Headoffice:
                            return "WpfMedaSlovenija.tblHeadofficeSlovenia";
                        default:
                            return null;
                    }
                default:
                    return null;
            }
        }

        private void dgHeadoffice_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (dgHeadoffice.SelectedIndex>-1)
            {
                Type t = Type.GetType(GetClassName(objInstitution.ctrID, TableName.Headoffice));
                var headoffice = Activator.CreateInstance(Type.GetType(GetClassName(objInstitution.ctrID, TableName.Headoffice)));
                headoffice= dgHeadoffice.SelectedItem;

                txtHeadofficeID.Text = headoffice.GetType().GetProperty("hdoID").GetValue(headoffice, null).ToString();
                txtHeadofficeName.Text= headoffice.GetType().GetProperty("hdoName").GetValue(headoffice, null).ToString();
            }
        }

        private bool ValidateFields()
        {
            if (String.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("Unesi ime institucije");
                return false;
            }
            if (String.IsNullOrWhiteSpace(txtHeadofficeID.Text))
            {
                MessageBox.Show("Unesi ime headoffice-a");
                return false;
            }
            if (String.IsNullOrWhiteSpace(txtHeadofficeName.Text))
            {
                MessageBox.Show("Unesi ime headoffice-a");
                return false;
            }
            if (cmbCity.SelectedIndex==0)
            {
                MessageBox.Show("Odaberi grad");
                return false;
            }
            if (cmbSubType.SelectedIndex==0)
            {
                MessageBox.Show("Odaberi tip institucije");
                return false;
            }
            return true;
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (!String.IsNullOrWhiteSpace(txtSearchHeadoffice.Text))
            {
                object _headoffice = Activator.CreateInstance(Type.GetType(GetClassName(objInstitution.ctrID, TableName.Headoffice)));
                PropertyInfo propertyInfo = _headoffice.GetType().GetProperty("hdoName");
                propertyInfo.SetValue(_headoffice,Convert.ChangeType(txtSearchHeadoffice.Text, propertyInfo.PropertyType),null);
                //_headoffice.GetType().GetProperty("hdoName").SetValue(_headoffice, txtHeadofficeName.Text);
                propertyInfo = _headoffice.GetType().GetProperty("staID");
                propertyInfo.SetValue(_headoffice, Convert.ChangeType(1, propertyInfo.PropertyType), null);
                //_headoffice.GetType().GetProperty("staID").SetValue(_headoffice, 1);
                int index = DataAccessLayer.AddHeadoffice(_headoffice,objInstitution.ctrID);
                if (index > 0)
                {
                    txtHeadofficeID.Text = index.ToString();
                    txtHeadofficeName.Text = txtSearchHeadoffice.Text;
                    txtSearchHeadoffice.Clear();
                    SetHeadoffices();
                }
            }
            else
            {
                MessageBox.Show("Unesi ime headoffice-a");
                return;
            }
        }
        #endregion
    }
}
