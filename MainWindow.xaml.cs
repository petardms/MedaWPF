using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.SqlServer;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Reflection;
using System.IO;
using Microsoft.Win32;
using Microsoft.Office.Interop.Excel;

namespace WpfMedaSlovenija
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : System.Windows.Window
    {
        public MainWindow()
        {
            InitializeComponent();

        }

        private void PopulateDatagrid(int ctrID)
        {
            using (MedaEntitiesDb context = new MedaEntitiesDb())
            {
                if (String.IsNullOrWhiteSpace(txtSearch.Text))
                    dgInstitution.ItemsSource = context.vwInstitutions.Where(n => n.ctrID == ctrID).ToList();
                else
                    GetSearchResult();
            }

        }


        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            using (MedaEntitiesDb context = new MedaEntitiesDb())
            {
                cmbCountry.ItemsSource = context.tblCountries.ToList();
                cmbCountry.DisplayMemberPath = "ctrName";
                cmbCountry.SelectedValuePath = "ctrID";
                cmbCountry.SelectedIndex = 2;
                PopulateDatagrid(Int32.Parse(cmbCountry.SelectedValue.ToString()));
                //Type obj = Type.GetType("vwInstitutionCroatia");
                //var q1 = Activator.CreateInstance(Type.GetType("WpfMedaSlovenija.vwInstitutionCroatia"));
                //var listQ1 = new List<typeof("WpfMedaSlovenija.vwInstitutionCroatia")>;

            }
        }
        //    var range = context.vwInstitutionCroatias.Where(n => n.insName.Contains("brod")).ToList();
        //    Type elementType = Type.GetType("WpfMedaSlovenija.vwInstitutionCroatia");
        //    Type listType = typeof(List<>).MakeGenericType(new Type[] { elementType });
        //    object query4 = Activator.CreateInstance(listType);
        //    listType.InvokeMember("AddRange", BindingFlags.InvokeMethod | BindingFlags.Instance | BindingFlags.Public, null, query4, new object[] { range
        //});
        //                        //PropertyInfo p = listType.GetProperty("AddRange");
        //                        var l = context.vwInstitutionCroatias.ToList();
        ////p.SetValue(query4, range,null);
        //var query1 = new List<vwInstitutionCroatia>();

        ////
        //Type tableType = Type.GetType("WpfMedaSlovenija.MedaEntitiesDb");
        //var tbl = Activator.CreateInstance(tableType);

        ////

        //Type typeContext = Type.GetType("WpfMedaSlovenija");
        //PropertyInfo p = typeContext.GetProperty("vwInstitutionCroatias");
        //var param = System.Linq.Expressions.Expression.Parameter(Type.GetType("WpfMedaSlovenija.vwInstitutionCroatia"));
        //var condition = System.Linq.Expressions.Expression.Lambda<Func<vwInstitutionCroatia, bool>>(
        //               System.Linq.Expressions.Expression.Equal(
        //               System.Linq.Expressions.Expression.Property(param, "insName"),
        //               System.Linq.Expressions.Expression.Constant(s, typeof(string))
        //               ),
        //               param);
        //var condition1 = System.Linq.Expressions.Expression.Lambda<Func<vwInstitutionCroatia, bool>>(
        //    System.Linq.Expressions.Expression.Call(
        //        System.Linq.Expressions.Expression.Constant(s),
        //        typeof(ICollection<Boolean>).GetMethod("Contains"),
        //        System.Linq.Expressions.Expression.Property(param, "insName"))
        //    , param);
        //var item1 = context.vwInstitutionCroatias.Where(condition1);
        //var item = context.vwInstitutionCroatias.SingleOrDefault(condition);


        private void GetSearchResult()
        {
            string textSearch = txtSearch.Text;
            int counter = 0;
            using (MedaEntitiesDb context = new MedaEntitiesDb())
            {
                if (String.IsNullOrWhiteSpace(txtSearch.Text))
                {
                    dgInstitution.ItemsSource = null;
                    PopulateDatagrid(Int32.Parse(cmbCountry.SelectedValue.ToString()));
                    return;
                }
                List<vwInstitution> query1 = new List<vwInstitution>();
                if (textSearch.Contains("@"))
                {
                    string[] arraySearch = textSearch.Split('@');
                    foreach (string s in arraySearch)
                    {
                        System.Text.RegularExpressions.Regex regEx = new System.Text.RegularExpressions.Regex(s);
                        switch (counter)
                        {
                            case 0:

                                if (s.Length > 0) query1.AddRange(context.vwInstitutions.Where(n => n.ctrID == (int)cmbCountry.SelectedValue).Where(n => SqlFunctions.PatIndex(s, n.insName) > 0).ToList());
                                break;
                            case 1:
                                if (s.Length > 0) query1.AddRange(context.vwInstitutions.Where(n => n.ctrID == (int)cmbCountry.SelectedValue).Where(n => SqlFunctions.PatIndex(s, n.ctyName) > 0).ToList());
                                break;
                            case 2:
                                if (s.Length > 0) query1.AddRange(context.vwInstitutions.Where(n => n.ctrID == (int)cmbCountry.SelectedValue).Where(n => SqlFunctions.PatIndex(s, n.insAddress) > 0).ToList());
                                break;
                            case 3:
                                if (s.Length > 0) query1.AddRange(context.vwInstitutions.Where(n => n.ctrID == (int)cmbCountry.SelectedValue).Where(n => SqlFunctions.PatIndex(s, n.hdoName) > 0).ToList());
                                break;
                            default:
                                break;
                        }

                        counter++;
                    }
                    dgInstitution.ItemsSource = null;
                    dgInstitution.ItemsSource = query1.ToList();
                }
                else
                {
                    if (textSearch.Length > 0)
                    {
                        query1.AddRange(context.vwInstitutions.Where(n => n.ctrID == (int)cmbCountry.SelectedValue).Where(n => SqlFunctions.PatIndex(txtSearch.Text, n.insName) > 0).ToList());
                        query1.AddRange(context.vwInstitutions.Where(n => n.ctrID == (int)cmbCountry.SelectedValue).Where(n => SqlFunctions.PatIndex(txtSearch.Text, n.ctyName) > 0).ToList());
                        query1.AddRange(context.vwInstitutions.Where(n => n.ctrID == (int)cmbCountry.SelectedValue).Where(n => SqlFunctions.PatIndex(txtSearch.Text, n.insAddress) > 0).ToList());
                        query1.AddRange(context.vwInstitutions.Where(n => n.ctrID == (int)cmbCountry.SelectedValue).Where(n => SqlFunctions.PatIndex(txtSearch.Text, n.hdoName) > 0).ToList());
                        dgInstitution.ItemsSource = null;
                        dgInstitution.ItemsSource = query1.Distinct().ToList();

                    }
                }
            }
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            GetSearchResult();

            //string[] _queryArray=

        }

        private void txtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                GetSearchResult();
            }
        }

        private void dgInstitution_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F)
            {
                txtSearch.Focus();
                return;
            }
            string className = null;
            switch (cmbCountry.SelectedValue.ToString())
            {
                case "1":
                    className = "vwInstitutionSerbia";
                    break;
                case "3":
                    className = "vwInstitutionCroatia";
                    break;
                case "4":
                    className = "vwInstitutionSlovenia";
                    break;
                default:
                    break;
            }
            if ((e.Key == Key.LeftCtrl && e.Key == Key.C) || (e.Key == Key.LeftShift || e.Key == Key.RightShift))
            {
                StringBuilder s = new StringBuilder();
                Type t = Type.GetType("WpfMedaSlovenija." + className);
                var row = Activator.CreateInstance(Type.GetType("WpfMedaSlovenija." + className));
                //vwInstution row = new vwInstution();
                if (dgInstitution.SelectedIndex > -1)
                {
                    //row = (vwInstution)dgInstitution.SelectedItem;
                    row = dgInstitution.SelectedItem;
                    List<PropertyInfo> listProperties = t.GetProperties().ToList();

                    s.Append(row.GetType().GetProperty("insID").GetValue(row, null) + "\t");
                    s.Append(row.GetType().GetProperty("insName").GetValue(row) + "\t");
                    s.Append(row.GetType().GetProperty("ctyName").GetValue(row) + "\t");
                    s.Append(row.GetType().GetProperty("insAddress").GetValue(row));
                    //s.Append(t.GetProperty("hdoName").GetValue(row));
                    //s.Append(row.insID + "\t");
                    //s.Append(row.insName + "\t");
                    //s.Append(row.ctyName + "\t");
                    //s.Append(row.insAddress + "\t");
                    //s.Append(row.hdoName);
                }
                Clipboard.SetText(s.ToString());
            }

            if (Keyboard.IsKeyDown(Key.LeftCtrl) && Keyboard.IsKeyDown(Key.N))
            {
                //Type t = Type.GetType("WpfMedaSlovenija." + className);
                //var selectedInstitution = Activator.CreateInstance(Type.GetType("WpfMedaSlovenija." + className));
                NewInstitution ni = new NewInstitution();
                if (dgInstitution.SelectedIndex > -1)
                {
                    vwInstitution vwi = (vwInstitution)dgInstitution.SelectedItem;
                    ni.objInstitution.insName = vwi.insName;
                    ni.objInstitution.insAddress = vwi.insAddress;
                    ni.objInstitution.ctyID = vwi.ctyID;
                    ni.objInstitution.ctyName = vwi.ctyName;
                    ni.objInstitution.isbID = vwi.isbID;
                    ni.objInstitution.isbName = vwi.isbName;
                    ni.objInstitution.itpID = vwi.itpID;
                    ni.objInstitution.itpName = vwi.itpName;
                    ni.objInstitution.hdoID = vwi.hdoID;
                    ni.objInstitution.hdoName = vwi.hdoName;
                    ni.objInstitution.ctrID = vwi.ctrID;
                }

                ni.ShowDialog();
                if (ni.result)
                {
                    PopulateDatagrid(Int32.Parse(cmbCountry.SelectedValue.ToString()));
                    MedaEntitiesDb mdb = new MedaEntitiesDb();
                    dgInstitution.SelectedItem = mdb.vwInstitutions.FirstOrDefault(n => n.insID == ni.index);
                    mdb.Dispose();
                }
            }
            if (Keyboard.IsKeyDown(Key.LeftCtrl) && Keyboard.IsKeyDown(Key.E))
            {
                //Type t = Type.GetType("WpfMedaSlovenija." + className);
                //var selectedInstitution = Activator.CreateInstance(Type.GetType("WpfMedaSlovenija." + className));
                NewInstitution ni = new NewInstitution();
                if (dgInstitution.SelectedIndex > -1)
                {
                    ni.objInstitution = (vwInstitution)dgInstitution.SelectedItem;

                }

                ni.ShowDialog();
                if (ni.result)
                {
                    PopulateDatagrid(Int32.Parse(cmbCountry.SelectedValue.ToString()));
                    MedaEntitiesDb mdb = new MedaEntitiesDb();
                    dgInstitution.SelectedItem = mdb.vwInstitutions.FirstOrDefault(n => n.insID == ni.index);
                    mdb.Dispose();
                }
            }
        }

        private void btnNew_Click(object sender, RoutedEventArgs e)
        {
            vwInstitution newInstitution = new vwInstitution();
            newInstitution.ctrID = (int)cmbCountry.SelectedValue;
            NewInstitution ni = new NewInstitution { objInstitution = newInstitution };
            ni.ShowDialog();
            if (ni.result)
            {
                PopulateDatagrid(Int32.Parse(cmbCountry.SelectedValue.ToString()));
                MedaEntitiesDb mdb = new MedaEntitiesDb();
                dgInstitution.SelectedItem = mdb.vwInstitutions.FirstOrDefault(n => n.insID == ni.index);
                mdb.Dispose();
            }
        }

        private void cmbCountry_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbCountry.SelectedIndex > -1)
                PopulateDatagrid(Int32.Parse(cmbCountry.SelectedValue.ToString()));
            else
                PopulateDatagrid(0);
        }

        private void btnExport_Click(object sender, RoutedEventArgs e)
        {
            if (dgInstitution.Items.Count == 0) return;

            SaveFileDialog sfd = new SaveFileDialog();
            sfd.InitialDirectory = "C:\\Users\\DIKA\\Desktop\\Meda";
            sfd.RestoreDirectory = true;
            //sfd.DefaultExt = "xlsx";
            sfd.Filter = "Excel Workbook|*.xlsx|Excel 97-2003 Workbook|*.xls|Text (Tab delimited)|*.txt|CSV (Semicolon delimited)|*.csv";
            sfd.FilterIndex = 1;
            sfd.OverwritePrompt = true;
            string xlPath;
            string xlPathBackground;
            string delim;
            Microsoft.Office.Interop.Excel.XlFileFormat fileFormat = new Microsoft.Office.Interop.Excel.XlFileFormat();
            bool? rez = sfd.ShowDialog();

            if (rez != true)
            {
                return;
            }
            else
                xlPath = sfd.FileName.ToString();

            string extension = Path.GetExtension(sfd.FileName).ToString();

            switch (extension.ToLower())
            {
                case ".csv":
                    delim = ";";
                    xlPathBackground = xlPath;
                    break;
                case ".xlsx":
                    delim = "\t";
                    xlPathBackground = Path.GetTempPath() + Path.GetFileNameWithoutExtension(sfd.FileName) + ".txt";
                    fileFormat = Microsoft.Office.Interop.Excel.XlFileFormat.xlWorkbookDefault;
                    break;
                case ".xls":
                    delim = "\t";
                    xlPathBackground = Path.GetFileNameWithoutExtension(sfd.FileName) + ".txt";
                    fileFormat = Microsoft.Office.Interop.Excel.XlFileFormat.xlExcel8;
                    break;
                default:
                    delim = "\t";
                    xlPathBackground = xlPath;
                    break;
            }

            using (StreamWriter sw = new StreamWriter(Path.GetFullPath(xlPathBackground), false))
            {
                sw.Write("ID\t");
                sw.Write("Name\t");
                sw.Write("City\t");
                sw.Write("Address\t");
                sw.Write("ctyID\t");
                sw.Write("dstName\t");
                sw.Write("dstID\t");
                sw.Write("Type\t");
                sw.Write("Subtype\t");
                sw.Write("Headoffice\t");
                sw.WriteLine();
                foreach (vwInstitution drv in dgInstitution.Items)
                {
                    sw.Write(drv.insID.ToString() + "\t");
                    sw.Write(drv.insName.ToString() + "\t");
                    sw.Write(drv.ctyName.ToString() + "\t");
                    sw.Write(drv.insAddress.ToString() + "\t");
                    sw.Write(drv.ctyID.ToString() + "\t");
                    sw.Write("\t");
                    sw.Write("\t");
                    sw.Write(drv.itpName.ToString() + "\t");
                    sw.Write(drv.isbName.ToString() + "\t");
                    sw.Write(drv.hdoName.ToString());
                    sw.WriteLine();
                }
            }

            if (extension.ToLower() == ".xlsx" || extension.ToLower() == ".xls")
            {
                Microsoft.Office.Interop.Excel.Application xlApp;
                Microsoft.Office.Interop.Excel.Workbook xlWorkBook;
                Microsoft.Office.Interop.Excel.Worksheet xlWorkSheet;

                object misValue = System.Reflection.Missing.Value;
                xlApp = new Microsoft.Office.Interop.Excel.Application();
                xlWorkBook = xlApp.Workbooks.Add(misValue);
                xlWorkSheet = (Microsoft.Office.Interop.Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);

                xlWorkBook.ActiveSheet.QueryTables.Add("TEXT;" + System.IO.Path.GetFullPath(xlPathBackground),
                (xlApp.ActiveWorkbook.Sheets[1]).Range("$A$1"), Type.Missing);

                xlWorkBook.ActiveSheet.QueryTables[1].Name = System.IO.Path.GetFileNameWithoutExtension(xlPathBackground);
                xlWorkBook.ActiveSheet.QueryTables[1].FieldNames = true;
                xlWorkBook.ActiveSheet.QueryTables[1].RowNumbers = false;
                xlWorkBook.ActiveSheet.QueryTables[1].FillAdjacentFormulas = false;
                xlWorkBook.ActiveSheet.QueryTables[1].PreserveFormatting = true;
                xlWorkBook.ActiveSheet.QueryTables[1].RefreshOnFileOpen = false;
                xlWorkBook.ActiveSheet.QueryTables[1].RefreshStyle = Microsoft.Office.Interop.Excel.XlCellInsertionMode.xlInsertDeleteCells;
                xlWorkBook.ActiveSheet.QueryTables[1].SavePassword = false;
                xlWorkBook.ActiveSheet.QueryTables[1].SaveData = true;
                xlWorkBook.ActiveSheet.QueryTables[1].AdjustColumnWidth = true;
                xlWorkBook.ActiveSheet.QueryTables[1].RefreshPeriod = 0;
                xlWorkBook.ActiveSheet.QueryTables[1].TextFilePromptOnRefresh = false;
                xlWorkBook.ActiveSheet.QueryTables[1].TextFilePlatform = 65001;
                xlWorkBook.ActiveSheet.QueryTables[1].TextFileStartRow = 1;
                xlWorkBook.ActiveSheet.QueryTables[1].TextFileParseType = Microsoft.Office.Interop.Excel.XlTextParsingType.xlDelimited;
                xlWorkBook.ActiveSheet.QueryTables[1].TextFileTextQualifier = Microsoft.Office.Interop.Excel.XlTextQualifier.xlTextQualifierDoubleQuote;
                xlWorkBook.ActiveSheet.QueryTables[1].TextFileConsecutiveDelimiter = false;
                xlWorkBook.ActiveSheet.QueryTables[1].TextFileTabDelimiter = true;
                xlWorkBook.ActiveSheet.QueryTables[1].TextFileSemicolonDelimiter = false;
                xlWorkBook.ActiveSheet.QueryTables[1].TextFileCommaDelimiter = false;
                xlWorkBook.ActiveSheet.QueryTables[1].TextFileSpaceDelimiter = false;
                //xlWorkBook.ActiveSheet.QueryTables[1].TextFileColumnDataTypes = GetColumnDataTypes(dt);

                xlWorkBook.ActiveSheet.QueryTables[1].RefreshStyle = Microsoft.Office.Interop.Excel.XlCellInsertionMode.xlInsertEntireRows;
                xlWorkBook.ActiveSheet.QueryTables[1].Refresh(false);
                xlApp.DisplayAlerts = false;
                xlWorkBook.SaveAs(xlPath, fileFormat,
                misValue, misValue, false, false, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlNoChange, misValue, misValue, misValue, misValue, misValue);
                if (File.Exists(xlPathBackground))
                    File.Delete(xlPathBackground);
                //xlApp.Visible = true;

                xlWorkBook.Close(true, xlPath);
                xlApp.Quit();

                releaseObject(xlWorkSheet);
                releaseObject(xlWorkBook);
                releaseObject(xlApp);
                MessageBox.Show("Done");
            }
        }
        private void releaseObject(object obj)
        {
            try
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(obj);
                obj = null;
            }
            catch (Exception ex)
            {
                obj = null;
                MessageBox.Show("Exception Occured while releasing object " + ex.ToString());
            }
            finally
            {
                GC.Collect();
            }
        }
    }
}
