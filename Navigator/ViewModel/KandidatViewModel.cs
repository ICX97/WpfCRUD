using Microsoft.Office.Interop.Excel;
using Navigator.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace Navigator.ViewModel
{
    public class KandidatViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private string tipPretraga = string.Empty;

        public string TipPretraga
        {
            get { return tipPretraga; }
            set
            {
                tipPretraga = value;
                OnPropertyChanged(tipPretraga);
            }
        }

        private string selectedPretraga = string.Empty;

        public string SelectedPretraga
        {
            get { return selectedPretraga; }
            set
            {
                selectedPretraga = value;
                OnPropertyChanged("SelectedPretraga");
                CheckIfEmpty();
            }
        }

        private void CheckIfEmpty()
        {
            if (selectedPretraga == "")
            {
                LoadKandidat();
            }
        }

        private Kandidat _kandidat;

        public Kandidat Kandidat
        {
            get { return _kandidat; }
            set { _kandidat = value;
                OnPropertyChanged(nameof(Kandidat));
            }
        }

        private ObservableCollection<Kandidat> tempKandidat = new ObservableCollection<Kandidat>();

        private ObservableCollection<Kandidat> _lstkandidat;

        public ObservableCollection<Kandidat> LstKandidat
        {
            get { return _lstkandidat; }
            set
            {
                _lstkandidat = value;
                OnPropertyChanged(nameof(LstKandidat));
            }
        }

        private Kandidat _newKandidat = new Kandidat();

        public Kandidat NewKandidat
        {
            get { return _newKandidat; }
            set {
                _newKandidat = value;
                OnPropertyChanged(nameof(NewKandidat));
            }
        }

        private Kandidat _selectedKandidat;

        public Kandidat SelectedKandidat
        {
            get { return _selectedKandidat; }
            set
            {
                _selectedKandidat = value;
                OnPropertyChanged(nameof(SelectedKandidat));
            }
        }


        navigatorEntities navEntity;
        public KandidatViewModel()
        {
            navEntity = new navigatorEntities();
            LoadKandidat();
            DeleteCommand = new Command((s) => true, Delete);
            UpdateCommand = new Command((s) => true, Update);
            UpdateKandidatCommand = new Command((s) => true, UpdateKandidat);
            AddKandidatCommand = new Command((s) => true, AddKandidat);
            PretraziCommand = new Command((s) => true, Pretrazi);
            ExportCommand = new Command((s) => true, Export);



        }

        private void Export(object obj)
        {
            Microsoft.Office.Interop.Excel.Application app = new Microsoft.Office.Interop.Excel.Application();
            app.Visible = true;
            app.WindowState = XlWindowState.xlMaximized;

            Workbook wb = app.Workbooks.Add(XlWBATemplate.xlWBATWorksheet);
            Worksheet ws = wb.Worksheets[1];
            int counter = 2;

            ws.EnableSelection = Microsoft.Office.Interop.Excel.XlEnableSelection.xlNoSelection;

            ws.Range["A1"].Value = "JMBG";
            ws.Range["B1"].Value = "Ime";
            ws.Range["C1"].Value = "Prezime";
            ws.Range["D1"].Value = "Godina rodjenja";
            ws.Range["E1"].Value = "Email";
            ws.Range["F1"].Value = "Telefon";
            ws.Range["G1"].Value = "Napomena";
            ws.Range["H1"].Value = "Zaposlen";
            ws.Range["I1"].Value = "Datum poslednje izmene";

            foreach (var i in LstKandidat)
            {
                
                ws.Range["A" + counter].Value = i.JMBG;
                ws.Range["B" + counter].Value = i.Ime;
                ws.Range["C" + counter].Value = i.Prezime;
                ws.Range["D" + counter].Value = i.GodinaRodjenja;
                ws.Range["E" + counter].Value = i.Email;
                ws.Range["F" + counter].Value = i.Telefon;
                ws.Range["G" + counter].Value = i.Napomena;
                ws.Range["H" + counter].Value = i.Zaposlen;
                ws.Range["I" + counter].Value = i.DatumPoslednjeIzmene;
                counter++;
                
            }


            wb.SaveAs("C:\\Users\\Caki\\source\\repos\\Navigator\\" + DateTime.Now.ToString("MM-dd-yyyy") + ".xlsx");
        }

        private void Pretrazi(object obj)
        {

            if (tipPretraga.Contains("JMBG") == true)
            {
                foreach (var kandidat in tempKandidat)
                {
                    if (!kandidat.JMBG.ToString().Contains(selectedPretraga))
                    {
                        LstKandidat.Remove(kandidat);
                    }
                }
            }
            else if(tipPretraga.Contains("Ime") == true)
            {
                foreach (var kandidat in tempKandidat)
                {
                    if (!kandidat.Ime.Contains(selectedPretraga))
                    {
                        LstKandidat.Remove(kandidat);
                    }
                }
            }
            else if(tipPretraga.Contains("Prezime") == true)
            {
                foreach (var kandidat in tempKandidat)
                {
                    if (!kandidat.Prezime.Contains(selectedPretraga))
                    {
                        LstKandidat.Remove(kandidat);
                    }
                }
            }
            else
            {

            }
        }

        private void UpdateKandidat(object obj)
        {
            
            navEntity.SaveChanges();
            SelectedKandidat.DatumPoslednjeIzmene = DateTime.Now;
            SelectedKandidat = new Kandidat();

        }
        private void Update(object obj)
        {
            SelectedKandidat = obj as Kandidat;

        }
        private void AddKandidat(object obj)
        {
            if (obj == null)
            {
                
            }
            else
            {
                NewKandidat.DatumPoslednjeIzmene = DateTime.Now;
                navEntity.Kandidats.Add(NewKandidat);
                navEntity.SaveChanges();
                LstKandidat.Add(NewKandidat);

                NewKandidat = new Kandidat();
            }
        }

        private void Delete(object obj)
        {
            var emp = obj as Kandidat;
            navEntity.Kandidats.Remove(emp);
            navEntity.SaveChanges();
            LstKandidat.Remove(emp);
        }


        private void LoadKandidat()
        {
            LstKandidat = new ObservableCollection<Kandidat>(navEntity.Kandidats);
            foreach (var kandidat in LstKandidat)
            {
                tempKandidat.Add(kandidat);
            }
        }
        public ICommand DeleteCommand { get; set; }
        public ICommand UpdateCommand { get; set; }
        public ICommand UpdateKandidatCommand { get; set; }
        public ICommand AddKandidatCommand { get; set; }
        public ICommand PretraziCommand { get; set; }

        public ICommand ExportCommand { get; set; }
    }
    class Command : ICommand
    {
        public Command(Func<object, bool> methodCanExecute, Action<object> methodExecute)
        {
            MethodCanExecute = methodCanExecute;
            MethodExecute = methodExecute;
        }
        Action<object> MethodExecute;
        Func<object, bool> MethodCanExecute;
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return MethodExecute != null && MethodCanExecute.Invoke(parameter);
        }
        public void Execute(object parameter)
        {
            MethodExecute(parameter);
        }
    }
}
