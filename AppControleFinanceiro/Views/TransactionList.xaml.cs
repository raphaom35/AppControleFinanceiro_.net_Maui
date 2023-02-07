using AppControleFinanceiro.Models;
using AppControleFinanceiro.Repositories;
using CommunityToolkit.Mvvm.Messaging;

namespace AppControleFinanceiro.Views;

public partial class TransactionList : ContentPage
{
    private TransactionAdd _transactionAdd;
    private TransactionEdit _transactionEdit;
    private ITransactionRepository _repository;
    public TransactionList(ITransactionRepository repository)
	{
        this._repository = repository;
		InitializeComponent();
        reload();
        WeakReferenceMessenger.Default.Register<string>(this, (e, msg) => {
            reload();
        });
	}
    public void reload()
    {
        var itens = _repository.GetAll();
        CollectionViewTransation.ItemsSource = itens;
        double income= itens.Where(a => a.Type == Models.TransactionType.Income).Sum(a => a.Value);
        double expenses = itens.Where(a => a.Type == Models.TransactionType.Expenses).Sum(a => a.Value);
        double balance = income - expenses;
        LabelIncome.Text = income.ToString("C");
        LabelExpense.Text = expenses.ToString("C");
        LabelBalance.Text = balance.ToString("C");
    }
    private void Button_Clicked_add(object sender, EventArgs e)
    {

        _transactionAdd= Handler.MauiContext.Services.GetService<TransactionAdd>();

        Navigation.PushModalAsync(_transactionAdd);
    }


    private void TapGestureRecognizer_Tapped_to_transation_edit(object sender, TappedEventArgs e)
    {
        var grid = ((Grid)sender).GestureRecognizers[0];
        var gestur =(TapGestureRecognizer)grid;
        Transaction transaction = (Transaction)gestur.CommandParameter;
        _transactionEdit = Handler.MauiContext.Services.GetService<TransactionEdit>();
        _transactionEdit.SetTransitionToEdit(transaction);
        Navigation.PushModalAsync(_transactionEdit);
    }

    private async void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {

        await AnimationBorder((Border)sender, true);
        bool result = await App.Current.MainPage.DisplayAlert("Excluir!", "Tem certeza que deseja excluir?", "Sim", "Não");

        if (result)
        {
            Transaction transaction = (Transaction)e.Parameter;
            _repository.Delete(transaction);

            reload();
        }
        else
        {
            await AnimationBorder((Border)sender, false);
        }
    }
    private Color _borderOriginalBackgroundColor;
    private String _labelOriginalText;
    private async Task AnimationBorder(Border border, bool IsDeleteAnimation)
    {
        var label = (Label)border.Content;

        if (IsDeleteAnimation)
        {
            _borderOriginalBackgroundColor = border.BackgroundColor;
            _labelOriginalText = label.Text;

            await border.RotateYTo(90, 500);

            border.BackgroundColor = Colors.Red;

            label.TextColor = Colors.White;
            label.Text = "X";

            await border.RotateYTo(180, 500);
        }
        else
        {
            await border.RotateYTo(90, 500);

            border.BackgroundColor = _borderOriginalBackgroundColor;
            label.TextColor = Colors.Black;
            label.Text = _labelOriginalText;

            await border.RotateYTo(0, 500);
        }
    }
}