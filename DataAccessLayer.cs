using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Windows.Data;
using System.Threading.Tasks;
using System.Windows;
using System.Reflection;

namespace WpfMedaSlovenija
{
    public class DataAccessLayer
    {
        public static int SaveInstitution(object _institution, int ctrID)
        {
            try
            {
                string className = null;

                switch (ctrID)
                {
                    case 1:
                        className = "tblInstitutionSerbia";

                        //_headoffice = (tblHeadofficeCroatia)_headoffice;
                        break;
                    case 3:
                        className = "tblInstitutionCroatia";
                        
                        //_headoffice = (tblHeadofficeCroatia)_headoffice;
                        break;
                    case 4:
                        className = "tblInstitutionSlovenia";
                        //_headoffice = (tblHeadofficeSlovenia)_headoffice;
                        break;
                    default:
                        break;
                }
                if (_institution!=null)
                {
                    using (MedaEntitiesDb mdb=new MedaEntitiesDb())
                    {
                        Type t = Type.GetType("WpfMedaSlovenija." + className);
                        dynamic ins = Cast(_institution, t);

                        //object _Institution = Activator.CreateInstance(Type.GetType("WpfMedaSlovenija." + className));
                        //_Institution = ins;
                        //mdb.SaveChanges();
                        //return ins.insID;
                        if (ins.insID==0)
                        {
                            //add
                            switch (ctrID)
                            {
                                case 1:
                                    mdb.tblInstitutionSerbias.Add(ins);
                                    mdb.SaveChanges();
                                    return mdb.tblInstitutionSerbias.Max(n => n.insID);
                                case 3:
                                    mdb.tblInstitutionCroatias.Add(ins);
                                    mdb.SaveChanges();
                                    return mdb.tblInstitutionCroatias.Max(n=>n.insID);
                                case 4:
                                    mdb.tblInstitutionSlovenias.Add(ins);
                                    mdb.SaveChanges();
                                    return mdb.tblInstitutionSlovenias.Max(n => n.insID);
                                default:
                                    return 0;
                            }
                        }
                        else
                        {
                            //edit
                            int i = ins.insID;
                            switch (ctrID)
                            {
                                case 1:
                                    tblInstitutionSerbia insSerbia = mdb.tblInstitutionSerbias.Where(n => n.insID == i).FirstOrDefault();
                                    insSerbia.insName = ins.insName;
                                    insSerbia.insAddress = ins.insAddress;
                                    insSerbia.isbID = ins.isbID;
                                    insSerbia.ctyID = ins.ctyID;
                                    insSerbia.hdoID = ins.hdoID;
                                    mdb.SaveChanges();
                                    return insSerbia.insID;
                                case 3:
                                    tblInstitutionCroatia insCoraita = mdb.tblInstitutionCroatias.Where(n => n.insID == i).FirstOrDefault();
                                    insCoraita.insName = ins.insName;
                                    insCoraita.insAddress = ins.insAddress;
                                    insCoraita.isbID = ins.isbID;
                                    insCoraita.ctyID = ins.ctyID;
                                    insCoraita.hdoID = ins.hdoID;
                                    mdb.SaveChanges();
                                    return insCoraita.insID;
                                case 4:
                                    tblInstitutionSlovenia insSlovenia = mdb.tblInstitutionSlovenias.Where(n => n.insID == i).FirstOrDefault();
                                    insSlovenia.insName = ins.insName;
                                    insSlovenia.insAddress = ins.insAddress;
                                    insSlovenia.isbID = ins.isbID;
                                    insSlovenia.ctyID = ins.ctyID;
                                    insSlovenia.hdoID = ins.hdoID;
                                    mdb.SaveChanges();
                                    return insSlovenia.insID;
                                default:
                                    return 0;
                            }
                        }
                    }
                }
                else
                {
                    return 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return 0;
            }
        }
        public static int AddHeadoffice(object _headoffice, int ctrID)
        {
            try
            {
                if (_headoffice != null)
                {
                    using (MedaEntitiesDb mdb = new MedaEntitiesDb())
                    {
                        switch (ctrID)
                        {
                            case 1:
                                mdb.tblHeadofficeSerbias.Add((tblHeadofficeSerbia)_headoffice);
                                mdb.SaveChanges();
                                return mdb.tblHeadofficeSerbias.Max(n => n.hdoID);
                            case 3:
                                mdb.tblHeadofficeCroatias.Add((tblHeadofficeCroatia)_headoffice);
                                mdb.SaveChanges();
                                return mdb.tblHeadofficeCroatias.Max(n => n.hdoID);
                            case 4:
                                mdb.tblHeadofficeSlovenias.Add((tblHeadofficeSlovenia)_headoffice);
                                mdb.SaveChanges();
                                return mdb.tblHeadofficeSlovenias.Max(n => n.hdoID);
                            default:
                                return 0;
                        }
                    }
                }
                else
                {
                    return 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return 0;
            }
        }

        public static int EditHeadoffice(object _headoffice, int ctrID)
        {
            try
            {
                string className = null;
                
                switch (ctrID)
                {
                    case 1:
                        className = "tblHeadofficeSerbia";

                        _headoffice = (tblHeadofficeSerbia)_headoffice;
                        break;
                    case 3:
                        className = "tblHeadofficeCroatia";
                        
                        _headoffice = (tblHeadofficeCroatia)_headoffice;
                        break;
                    case 4:
                        className = "tblHeadofficeSlovenia";
                        _headoffice = (tblHeadofficeSlovenia)_headoffice;
                        break;
                    default:
                        break;
                }
                if (_headoffice != null)
                {
                    using (MedaEntitiesDb mdb = new MedaEntitiesDb())
                    {
                        Type t = Type.GetType("WpfMedaSlovenija." + className);
                        dynamic hd = Cast(_headoffice, t);
                        object _Headoffice = Activator.CreateInstance(Type.GetType("WpfMedaSlovenija." + className));
                        //tblHeadoffice _Headoffice = mdb.tblHeadoffice.FirstOrDefault(n => n.hdoID == _headoffice.hdoID);
                        if (_Headoffice != null)
                        {
                            t.GetProperty("hdoName").SetValue(null, hd.hdoName);
                            t.GetProperty("staID").SetValue(null, hd.staID);
                            //    _Headoffice.hdoName = _headoffice.hdoName;
                            //_Headoffice.staID = _headoffice.staID;
                            mdb.SaveChanges();
                            return hd.hdoID;
                        }
                        else
                        {
                            return 0;
                        }
                    }
                }
                else
                {
                    return 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return 0;
            }
        }

        public static dynamic Cast(object obj, Type t)
        {
            if (obj is IConvertible)
            {
                return Convert.ChangeType(obj, t) as dynamic;
            }
            else
            {
                try
                {
                    var param = System.Linq.Expressions.Expression.Parameter(typeof(object));
                    return System.Linq.Expressions.Expression.Lambda(System.Linq.Expressions.Expression.Convert(param, t), param)
                        .Compile().DynamicInvoke(obj) as dynamic;
                }
                catch (TargetInvocationException ex)
                {
                    throw ex.InnerException;
                }
            }
        }
    }
}
