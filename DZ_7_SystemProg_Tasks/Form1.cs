using OpenNLP.Tools.SentenceDetect;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DZ_7_SystemProg_Tasks
{
	public partial class Form1 : Form
	{
		//Подключение спец библиотеки OpenNLP для анализа текста https://github.com/AlexPoint/OpenNlp/blob/master/Resources/Models/EnglishSD.nbin
		static EnglishMaximumEntropySentenceDetector sentenceDetector = new EnglishMaximumEntropySentenceDetector(Directory.GetCurrentDirectory() + @"\Resources\EnglishSD.nbin");
		string tempstr;

		public Form1()
		{
			InitializeComponent();

			//Проверка на корректность/потокобезопасность
			Control.CheckForIllegalCrossThreadCalls = true;
		}

		//Тестовый вывод содержимого сайта
		static Task<string> GetSiteStr()
		{
			WebClient w = new WebClient();
			return w.DownloadStringTaskAsync("http://ya.ru");
		}

		//Подсчет символов (async выдергивает тип данных из таска)
		async static Task<int> GetSymbolsNumber(string _tempstr)
		{
			int count = 0;
			count = _tempstr.Length;
			return count;
		}
		//Подсчет предложений
		async static Task<int> GetSentenceNumber(string _tempstr)
		{
			string[] sentences = sentenceDetector.SentenceDetect(_tempstr);
			return sentences.Count();
		}
		//Подсчет слов
		async static Task<int> GetWordsNumber(string _tempstr)
		{
			string[] words = _tempstr.Split(new char[] { ' ', '\n' }, StringSplitOptions.RemoveEmptyEntries);
			return words.Count();
		}
		//Подсчет восклицательных предложений
		async static Task<int> GetSentenceNumber111(string _tempstr)
		{
			int count = 0;
			string[] sentences = sentenceDetector.SentenceDetect(_tempstr);
			foreach (var item in sentences)
			{
				if (item.EndsWith("!"))
				{
					count++;
				}
			}
			return count;
		}
		//Подсчет вопросительных предложений
		async static Task<int> GetSentenceNumberQuestion(string _tempstr)
		{
			int count = 0;
			string[] sentences = sentenceDetector.SentenceDetect(_tempstr);
			foreach (var item in sentences)
			{
				if (item.EndsWith("?"))
				{
					count++;
				}
			}
			return count;
		}


		//Объявляем что метод является асинхронным
		private async void btn_analyze_Click(object sender, EventArgs e)
		{
			//Работа с сайтом
			//Работает
			//WebClient client = new WebClient();
			//richTextBox1.Text = client.DownloadString("http://ya.ru");

			//Работает асинхронно
			//await таск возвращает необходимый тип данных, то что  было в таске (string)
			//string data = await GetSiteStr();
			//richTextBox1.Text = data;

			try
			{
				tempstr = richTextBox1.Text;
				int number = await GetSymbolsNumber(tempstr);
				int sentNumber = await GetSentenceNumber(tempstr);
				int wordsNumber = await GetWordsNumber(tempstr);
				int sentNumber111 = await GetSentenceNumber111(tempstr);
				int sentNumberQuestion = await GetSentenceNumberQuestion(tempstr);

				DialogResult res = MessageBox.Show($"Количество символов в тексте равно {number} \nКоличество предложений в тексте равно {sentNumber} \nКоличество слов в тексте равно {wordsNumber}" +
					$"\nКоличество восклицательных предложений {sentNumber111}\nКоличество вопросительных предложений {sentNumberQuestion}\nСохранить результаты в файл?", "Результаты анализа текста", MessageBoxButtons.YesNoCancel);
				if (res == DialogResult.Yes)
				{
					string filename = "Results.txt";
					using (var sw = new StreamWriter(filename, false, Encoding.UTF8))
					{
						sw.WriteLine($"Количество символов в тексте равно {number}");
						sw.WriteLine($"Количество предложений в тексте равно {sentNumber}");
						sw.WriteLine($"Количество слов в тексте равно {wordsNumber}");
						sw.WriteLine($"Количество восклицательных предложений {sentNumber111}");
						sw.WriteLine($"Количество вопросительных предложений {sentNumberQuestion}");
						MessageBox.Show($"Файл Results.txt записан в директорию {Directory.GetCurrentDirectory()}");
					}
				}
			}
			catch (Exception)
			{
				MessageBox.Show("Некорректное преобразование текста");
			}
		}
		private void btn_close_Click(object sender, EventArgs e)
		{
			Close();
		}
	}
}
