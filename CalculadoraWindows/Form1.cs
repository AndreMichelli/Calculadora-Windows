using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CalculadoraWindows {
    public partial class FormCalculadora : Form {
        public FormCalculadora() {
            InitializeComponent();
        }
        //
        //
        // VARIAVEIS
        //
        //
        bool nvNum = false;
        bool fechaParentese = false;
        bool fe = false;
        bool hyperb = false;
        string deg = "deg";
        Panel penHist;
        Panel penMem;
        Label labelHist;
        Label labelHist2;
        Label labelMem;
        int nHist = 1;
        int nMem = 1;
        Label[] labelVetHist = new Label[0];
        Label[] labelVetHist2 = new Label[0];
        Label[] labelVetMem = new Label[0];

        //
        //
        // FUNÇÕES PARA O HISTÓRICO
        //
        //

        private void Historico(string num, string men) {
            penHist = new Panel();
            labelHist = new Label();
            labelHist2 = new Label();

            penHist.Height = 70;
            penHist.Dock = DockStyle.Top;
            penHist.MouseEnter += new EventHandler(PnFoco);
            penHist.MouseLeave += new EventHandler(ForaFoco);
            penHist.Click += new EventHandler(PnClickHist);
            penHist.Name = "pn" + nHist;

            labelHist.Text = men + " =";
            labelHist.Font = new Font("Segoe UI", 10);
            labelHist.Name = "label1" + nHist;
            labelHist.Left = panelHistorico.Width - labelHist.Width;

            labelHist2.Text = men + " =";
            labelHist2.Font = new Font("Segoe UI", 16, FontStyle.Bold);
            labelHist2.Top = 20;
            labelHist2.Height = 32;
            labelHist2.Name = "label2" + nHist;
            labelHist2.Left = panelHistorico.Width - labelHist.Width;

            penHist.Controls.Add(panelHistorico);
            penHist.Controls.Add(labelHist2);
            penHist.Controls.Add(labelHist);
            
            Array.Resize(ref labelVetHist, nHist);
            Array.Resize(ref labelVetHist, nHist);
            labelVetHist[nHist - 1] = labelHist;
            labelVetHist2[nHist - 1] = labelHist2;
            nHist += nHist;
        }
        private void PnFoco(object sender, EventArgs e) {
            Panel pn = sender as Panel;
            pn.BackColor = Color.DarkGray;
        }
        private void ForaFoco(object sender, EventArgs e) {
            Panel pn = sender as Panel;
            pn.BackColor = Color.Gray;
        }
        private void PnClickHist(object sender, EventArgs e) {
            Panel panel = sender as Panel;
            string a = panel.Name.Remove(0, 2);
            labelHist = labelVetHist[int.Parse(a) - 1];
            labelHist2 = labelVetHist2[int.Parse(a) - 1];

            labelResultado.Text = labelHist2.Text;
            labelConta.Text = labelHist.Text.Remove(labelHist.Text.Length - 2, 2);

            nvNum = true;

            fechaParentese = true;
        }

        //
        //
        // FUNÇÕES PARA A MEMÓRIA
        //
        //
        private void Memoria(string num) {
            penMem = new Panel();
            labelMem = new Label();

            penMem.Height = 70;
            penMem.Dock = DockStyle.Top;
            penMem.MouseEnter += new EventHandler(PnFoco);
            penMem.MouseLeave += new EventHandler(PnForaFoco);
            penMem.Click += new EventHandler(PnCliqueMem);
            penMem.Name = "pn" + nMem;

            labelMem.Text = num;
            labelMem.Font = new Font("Segoe UI", 16, FontStyle.Bold);
            labelMem.Name = "lb1_" + nMem;
            labelMem.Left = panelMemoria.Width - labelMem.Width;
            labelMem.Height = 35;

            panelMemoria.Controls.Add(penMem);
            penMem.Controls.Add(labelMem);

            Array.Resize(ref labelVetMem, nMem);
            labelVetMem[nMem - 1] = labelMem;
            nMem++;
        }

        private void PnCliqueMem(object sender, EventArgs e) {
            Panel panel = sender as Panel;
            string a = panel.Name.Remove(0, 2);
            labelMem = labelVetMem[int.Parse(a) - 1];

            labelResultado.Text = labelMem.Text;

            nvNum = false;
            fechaParentese = false;
        }
        //
        //
        // RESPONSIVIDADE
        //
        //
        private void FormCalculadora_SizeChanged(object sender, EventArgs e) {
            if (this.Width < 800) {
                splitTela.Panel2Collapsed = true;
            }
            else {
                splitTela.Panel2Collapsed = false;
            }
        }

        //
        //
        // FAZ CONTAS
        //
        //
        private double Evaluete(string expression) {
            expression = expression.Replace(",", ".");
            expression = expression.Replace("÷", "/");
            expression = expression.Replace("×", "*");
            try {
                System.Data.DataTable table = new System.Data.DataTable();
                table.Columns.Add("expression", string.Empty.GetType(), expression);
                System.Data.DataRow row = table.NewRow();
                table.Rows.Add(row);
                return double.Parse((string)row["expression"]);
            }
            catch {
                return double.Parse("0");
            }

        }
        private string DecimalToDegree(string dec) {
            double decimal_degrees = double.Parse(dec);
            int cont = int.Parse(((decimal_degrees - Math.Floor(decimal_degrees)) / 0.6).ToString().Split(',')[0]);
            string degree = (cont + Math.Floor(decimal_degrees)).ToString() + "," + ((decimal_degrees - Math.Floor(decimal_degrees)) / 0.6).ToString().Split(',')[1];
            return degree;
        }
        private string DecimalToMinutes(string dec) {
            double decimal_degrees = double.Parse(dec);
            int cont = int.Parse(((decimal_degrees - Math.Floor(decimal_degrees)) * 0.6).ToString().Split(',')[0]);
            string degree = (cont + Math.Floor(decimal_degrees)).ToString() + "," + ((decimal_degrees - Math.Floor(decimal_degrees)) / 0.6).ToString().Split(',')[1];
            return degree;
        }
        //
        //
        // BOTÕES CALCULADORA COMUM
        //
        //
        private void btnNum_Click(object sender, EventArgs e) {
            Button btn = sender as Button;
            if (labelResultado.Text == "0" || nvNum) {
                labelResultado.Text = "";
                nvNum = false;
            }
            labelResultado.Text += btn.Text;
        }

        private void btnOper_Click(object sender, EventArgs e) {
            Button btn = sender as Button;
            string aux = labelResultado.Text;

            if (fe) {
                aux = double.Parse(labelResultado.Text).ToString("0.###E+");
            }
            else {
                aux = labelResultado.Text;
            }
            if (fe) {
                labelResultado.Text = Evaluete(labelConta.Text + labelResultado.Text).ToString("0.###E+");
            }
            else { 
                labelResultado.Text = Evaluete(labelConta.Text + labelResultado.Text).ToString();
            }
            if (fechaParentese) {
                labelConta.Text += " " + btn.Text + " ";
            }
            else {
                labelConta.Text += aux + " " + btn.Text + " ";
            }

            nvNum = true;
        }

        private void btnCE_Click(object sender, EventArgs e) {
            labelResultado.Text = "0";
        }

        private void btnC_Click(object sender, EventArgs e) {
            labelResultado.Text = "0";
            labelConta.Text = "";
        }

        private void btnApagar_Click(object sender, EventArgs e) {
            labelResultado.Text = labelResultado.Text.Substring(0, labelResultado.Text.Length - 1);
        }

        private void btnVirg_Click(object sender, EventArgs e) {
            if (!labelResultado.Text.Contains(',')) {
                labelResultado.Text += ",";
            }
        }

        private void btnMaisMns_Click(object sender, EventArgs e) {
            labelResultado.Text = (double.Parse(labelResultado.Text) * -1).ToString();
        }

        private void btnIgual_Click(object sender, EventArgs e) {
            string conta = labelConta.Text + labelResultado.Text;
            double resultado = Evaluete(conta);
            if (fe)
                labelResultado.Text = resultado.ToString("0.###E+0");
            else
                labelResultado.Text = resultado.ToString();
            Historico(resultado.ToString(), conta);
            labelConta.Text = "";
            nvNum = true;
        }

        //
        //
        // BOTÕES CALCULADORA CIENTIFICA
        //
        //
        private void btnXquad_Click(object sender, EventArgs e) {
            labelResultado.Text = Math.Pow(double.Parse(labelResultado.Text), 2).ToString();
            nvNum = true;
        }

        private void btnXelevY_Click(object sender, EventArgs e) {
            string aux = labelResultado.Text;

            labelResultado.Text = Evaluete(labelConta.Text + labelResultado.Text).ToString();

            labelConta.Text += aux + " ^ ";
            nvNum = true;
        }

        private void btnSin_Click(object sender, EventArgs e) {

            if (hyperb) {
                labelResultado.Text = Math.Sinh(double.Parse(labelResultado.Text)).ToString();
                nvNum = true;
            }
            else { 
            labelResultado.Text = Math.Sin(double.Parse(labelResultado.Text)).ToString();
            nvNum = true;
            }
        }

        private void btnCos_Click(object sender, EventArgs e) {
            if (hyperb) {
                labelResultado.Text = Math.Cosh(double.Parse(labelResultado.Text)).ToString();
                nvNum = true;
            }
            else {
                labelResultado.Text = Math.Cos(double.Parse(labelResultado.Text)).ToString();
                nvNum = true;
            }
        }

        private void btnTan_Click(object sender, EventArgs e) {
            if (hyperb) {
                labelResultado.Text = Math.Tanh(double.Parse(labelResultado.Text)).ToString();
                nvNum = true;
            }
            else {
                labelResultado.Text = Math.Tan(double.Parse(labelResultado.Text)).ToString();
                nvNum = true;
            }
        }

        private void btnXcubo_Click(object sender, EventArgs e) {
            labelResultado.Text = Math.Pow(double.Parse(labelResultado.Text), 3).ToString();
            nvNum = true;
        }

        private void btnYraizX_Click(object sender, EventArgs e) {
            string aux = labelResultado.Text;
            labelResultado.Text = Evaluete(labelConta.Text + labelResultado.Text).ToString();
            labelConta.Text += aux + " yroot ";
            nvNum = true;
        }

        private void btnSinMns1_Click(object sender, EventArgs e) {
            if (hyperb) {
                labelResultado.Text = Math.Pow(Math.Sinh(double.Parse(labelResultado.Text)),-1).ToString();
                nvNum = true;
            }
            else {
                labelResultado.Text = Math.Asin(double.Parse(labelResultado.Text)).ToString();
                nvNum = true;
            }
        }

        private void btnCosMns1_Click(object sender, EventArgs e) {
            if (hyperb) {
                labelResultado.Text = Math.Pow(Math.Cosh(double.Parse(labelResultado.Text)), -1).ToString();
                nvNum = true;
            }
            else {
                labelResultado.Text = Math.Acos(double.Parse(labelResultado.Text)).ToString();
                nvNum = true;
            }
        }

        private void btnTanMns1_Click(object sender, EventArgs e) {
            if (hyperb) {
                labelResultado.Text = Math.Pow(Math.Tanh(double.Parse(labelResultado.Text)), -1).ToString();
                nvNum = true;
            }
            else {
                labelResultado.Text = Math.Atan(double.Parse(labelResultado.Text)).ToString();
                nvNum = true;
            }
        }

        private void btnRaiz_Click(object sender, EventArgs e) {
            labelResultado.Text = Math.Sqrt(double.Parse(labelResultado.Text)).ToString();
            nvNum = true;
        }

        private void btn10elevX_Click(object sender, EventArgs e) {
            labelResultado.Text = Math.Pow(10, double.Parse(labelResultado.Text)).ToString();
            nvNum = true;
        }

        private void btnLog_Click(object sender, EventArgs e) {
            labelResultado.Text = Math.Log(double.Parse(labelResultado.Text)).ToString();
            nvNum = true;
        }

        private void btnExp_Click(object sender, EventArgs e) {
            string resultado = labelResultado.Text;
            if (resultado.Contains(',')) { 
            labelResultado.Text = resultado + "E+";
            }
            else {
                labelResultado.Text = resultado + ",E+";
            }
        }

        private void btnMod_Click(object sender, EventArgs e) {
            string aux = labelResultado.Text;

            labelResultado.Text = Evaluete(labelConta.Text + labelResultado.Text).ToString();

            labelConta.Text += aux + " Mod ";
            nvNum = true;
        }

        private void btn1sobX_Click(object sender, EventArgs e) {
            labelResultado.Text = (1/double.Parse(labelResultado.Text)).ToString();
            nvNum = true;
        }

        private void btnEaX_Click(object sender, EventArgs e) {
            labelResultado.Text = Math.Pow(Math.E, double.Parse(labelResultado.Text)).ToString();
            nvNum = true;
        }

        private void btnLn_Click(object sender, EventArgs e) {
            labelResultado.Text = (Math.Log(double.Parse(labelResultado.Text))/Math.Log(Math.E)).ToString();
            nvNum = true;
        }

        private void btnDms_Click(object sender, EventArgs e) {
            labelResultado.Text = DecimalToMinutes(labelResultado.Text);
            nvNum = true;
        }

        private void btnGraus_Click(object sender, EventArgs e) {
            labelResultado.Text = DecimalToDegree(labelResultado.Text);
            nvNum = true;
        }

        private void btnPi_Click(object sender, EventArgs e) {
            labelResultado.Text = Math.PI.ToString();
            nvNum = true;
        }

        private void btnFat_Click(object sender, EventArgs e) {
            double num = double.Parse(labelResultado.Text);
            double fatorial = 1;
            for (double i = num; i > 0; i--) {
                fatorial *= i;
            }
            labelResultado.Text = fatorial.ToString();
            nvNum = true;
        }

        private void btnAbreParent_Click(object sender, EventArgs e) {
            labelConta.Text += " ( ";
            nvNum = true;
        }

        private void btnFechaParen_Click(object sender, EventArgs e) {
            string aux = labelResultado.Text;
            labelConta.Text += aux + " ) ";
            nvNum = true;
            fechaParentese = true;
        }

        private void btnFE_Click(object sender, EventArgs e) {
            if (fe) {
                fe = false;
                btnFE.FlatAppearance.BorderSize = 0;
                labelResultado.Text = double.Parse(labelResultado.Text).ToString();
            }
            else {
                fe = true;
                btnFE.FlatAppearance.BorderColor = Color.Orange;
                btnFE.FlatAppearance.BorderSize = 1;
                labelResultado.Text = double.Parse(labelResultado.Text).ToString("0.###E+");
            }
        }

        private void btnHyp_Click(object sender, EventArgs e) {
            if (hyperb) {
                hyperb = false;
                btnHyp.FlatAppearance.BorderSize = 0;
            }
            else {
                hyperb = true;
                btnHyp.FlatAppearance.BorderColor = Color.Orange;
                btnHyp.FlatAppearance.BorderSize = 1;
                labelResultado.Text = double.Parse(labelResultado.Text).ToString("0.###E+");
            }
        }

        private void btnDegCima_Click(object sender, EventArgs e) {
            if (deg == "deg") {
                btnDegCima.Text = "RAD";
                deg = "rad";
            }
            else if (deg == "rad") {
                btnDegCima.Text = "GRAD";
                deg = "grad";
            }
            else {
                btnDegCima.Text = "DEG";
                deg = "deg";
            }
        }
    }

}
