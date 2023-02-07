

using AppControleFinanceiro.Models;
using AppControleFinanceiro.Repositories;
using CommunityToolkit.Mvvm.Messaging;
using System.Text;

namespace AppControleFinanceiro.Views;

public partial class TransactionAdd : ContentPage
{
    private ITransactionRepository _repository;
	public TransactionAdd(ITransactionRepository transactionRepository)
	{
        
		InitializeComponent();
        _repository = transactionRepository;
    }

    private void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        Navigation.PopModalAsync();
    }

    private void On_Button_Clicked_Save(object sender, EventArgs e)
    {
        if (IsvalidData() == false)
            return;
        SaveTransationIndatabase();
        Navigation.PopModalAsync();
        WeakReferenceMessenger.Default.Send<string>("");
        //var count =_repository.GetAll().Count;
        //App.Current.MainPage.DisplayAlert("Menssagem", $"Exitem {count} registro", "Ok");

    }

    private void SaveTransationIndatabase()
    {
        Transaction transaction = new Transaction()
        {
            Type = RadioIncome.IsChecked ? TransactionType.Income : TransactionType.Expenses,
            Name = EntryName.Text,
            Date = DatePickerDate.Date,
            Value = double.Parse(EntryValor.Text)
        };
        _repository.Add(transaction);
    }

    private bool IsvalidData()
    {
        StringBuilder sb = new StringBuilder();
        bool valid = true;
        if (string.IsNullOrEmpty(EntryName.Text) | string.IsNullOrWhiteSpace(EntryName.Text))
        {
            sb.Append("O campo ''Nome' deve ser preenchido \r\n");
            valid = false;
        }
        if (string.IsNullOrEmpty(EntryValor.Text) || string.IsNullOrWhiteSpace(EntryValor.Text))
        {
            sb.Append("O campo ''Valor' deve ser preenchido \r\n");
            valid = false;
        }
        double result;
        if (!string.IsNullOrEmpty(EntryValor.Text) && !double.TryParse(EntryValor.Text, out result))
        {
            sb.Append("O campo 'Valor' é invalido \r\n");
            valid = false;
        }
        if (valid == false)
        {
            LabelError.Text = sb.ToString();
            LabelError.IsVisible = true;
        }
        return valid;
    }
}