using System;
using System.Data;
using System.IO;
using System.Windows.Forms;

namespace Calculator
{
    public partial class Form1 : Form
    {
        private string historyFilePath = "calculator_history.txt";

        public Form1()
        {
            InitializeComponent();
            LoadHistory();
        }

        private void buttonClick(object sender, EventArgs e)
        {
            var currentButton = sender as Button;
            textBox1.Text += currentButton.Text;
        }

        private void button12_Click(object sender, EventArgs e) // Кнопка "="
        {
            try
            {
                if (string.IsNullOrEmpty(textBox1.Text))
                {
                    MessageBox.Show("Введите выражение для вычисления", "Ошибка",
                                  MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string expression = textBox1.Text.Replace(",", ".");
                var result = new DataTable().Compute(expression, null);

                string resultString = result.ToString();
                textBox1.Text = resultString;

                // Сохраняем в историю
                SaveToHistory($"{expression} = {resultString}");
            }
            catch (SyntaxErrorException)
            {
                MessageBox.Show("Некорректное математическое выражение", "Ошибка",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (EvaluateException)
            {
                MessageBox.Show("Ошибка вычисления выражения", "Ошибка",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button11_Click(object sender, EventArgs e) // Кнопка "←"
        {
            if (!string.IsNullOrEmpty(textBox1.Text))
            {
                textBox1.Text = textBox1.Text.Substring(0, textBox1.Text.Length - 1);
            }
        }

        private void button17_Click(object sender, EventArgs e) // Кнопка "C"
        {
            textBox1.Text = "";
        }

        private void SaveToHistory(string entry)
        {
            try
            {
                File.AppendAllText(historyFilePath, $"{DateTime.Now}: {entry}\n");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Не удалось сохранить в историю: {ex.Message}", "Ошибка",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadHistory()
        {
            if (File.Exists(historyFilePath))
            {
                try
                {
                    History.Items.AddRange(File.ReadAllLines(historyFilePath));
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Не удалось загрузить историю: {ex.Message}", "Ошибка",
                                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void button19_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(textBox1.Text))
            {
                SaveToHistory($"Результат: {textBox1.Text}");
                MessageBox.Show("Результат сохранен в истории", "Сохранено",
                              MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void button20_Click(object sender, EventArgs e)
        {
            if (History.SelectedItem != null)
            {
                string selectedItem = History.SelectedItem.ToString();
                int equalSignIndex = selectedItem.IndexOf('=');

                if (equalSignIndex > 0)
                {
                    string result = selectedItem.Substring(equalSignIndex + 1).Trim();
                    textBox1.Text = result;
                }
            }
        }

        private void button21_Click(object sender, EventArgs e)
        {
            try
            {
                File.Delete(historyFilePath);
                History.Items.Clear();
                MessageBox.Show("История очищена", "Успешно",
                              MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Не удалось очистить историю: {ex.Message}", "Ошибка",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}