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

        private string filtroAlimento;
        public string FiltroAlimento
        {
            get { return filtroAlimento; }
            set
            {
                filtroAlimento = value;
                OnPropertyChanged("FiltroAlimento");
                CVFuncoes?.Refresh();
            }
        }

        public ConsultaWindowDados(Microondas microondas)
        {
            filtroAlimento = "";
            CVFuncoes = CollectionViewSource.GetDefaultView(microondas.Funcoes);
            CVFuncoes.Filter = FiltrarFuncoes;
        }

        private bool FiltrarFuncoes(object item)
        {
            var filtro = filtroAlimento.Trim().ToLower();
            if (filtro.Length == 0)
                return true;

            var funcao = item as FuncaoMicroondas;
            if (funcao.Alimento == null || funcao.Alimento == "")
                return false;

            var res = funcao.Alimento.ToLower().Contains(filtro);
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
