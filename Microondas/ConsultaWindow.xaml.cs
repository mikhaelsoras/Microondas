using Classes.Microondas;
using System.ComponentModel;
using System.Windows;
using System.Windows.Data;

namespace MicroondasProject
{
    public class ConsultaWindowDados
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public ICollectionView CVFuncoes { get; private set; }

        private string filtroNome;
        public string FiltroNome
        {
            get { return filtroNome; }
            set
            {
                filtroNome = value;
                OnPropertyChanged("FiltroNome");
                CVFuncoes?.Refresh();
            }
        }

        public ConsultaWindowDados(Microondas microondas)
        {
            filtroNome = "";
            CVFuncoes = CollectionViewSource.GetDefaultView(microondas.Funcoes);
            CVFuncoes.Filter = FiltrarFuncoes;
        }

        private bool FiltrarFuncoes(object item)
        {
            var filtro = filtroNome.Trim().ToLower();
            if (filtro.Length == 0)
                return true;

            var funcao = item as FuncaoMicroondas;
            var res = funcao.Nome.ToLower().Contains(filtro);
            return res;
        }
    }

    public partial class ConsultaWindow : Window
    {
        ConsultaWindowDados dados;

        public ConsultaWindow(Microondas microondas)
        {
            InitializeComponent();

            dados = new ConsultaWindowDados(microondas);

            DataContext = dados;
        }
    }
}
