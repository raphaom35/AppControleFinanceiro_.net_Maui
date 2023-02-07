using AppControleFinanceiro.Models;
using AppControleFinanceiro.Repositories;
using CommunityToolkit.Mvvm.Messaging;
using System.Text;

namespace AppControleFinanceiro.Views;

public partial class TransactionEdit : ContentPage
{
    private  Transaction _transaction;
    private ITransactionRepository _repository;
    public TransactionEdit(ITransactionRepository transactionRepository)
	{
		InitializeComponent();
        _repository = transactionRepository;
    }
    public void SetTransitionToEdit(Transaction transaction)
    {
        _transaction=transaction;
        if(transaction.Type == TransactionType.Income)
        {
            RadioIncome.IsChecked=true;
        }
        else
        {
            RadioIncome.IsChecked = true;
        }
        EntryName.Text = transaction.Name;
        EntryValor.Text = transaction.Value.ToString();
        DatePickerDate.Date = transaction.Date.Date;
    }
    private void TapGestureRecognizer_Tapped_to_close(object sender, TappedEventArgs e)
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
        var count = _repository.GetAll().Count;
       
    }

    private void SaveTransationIndatabase()
    {
        Transaction transaction = new Transaction()
        {
            id = _transaction.id,
            Type = RadioIncome.IsChecked ? TransactionType.Income : TransactionType.Expenses,
            Name = EntryName.Text,
            Date = DatePickerDate.Date,
            Value = double.Parse(EntryValor.Text)
        };
        _repository.Update(transaction);
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