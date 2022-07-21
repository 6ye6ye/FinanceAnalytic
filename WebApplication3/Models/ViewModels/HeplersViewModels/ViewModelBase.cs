namespace FinanceAnalytic.Models.ViewModels
{
    public abstract class ViewModelBase<TModel> where TModel: class, new()
    {
        internal TModel Model { get; private set; }

        protected ViewModelBase(TModel model)
        {
            Model = model;
        }

        protected ViewModelBase()
        {
            Model = new TModel();
        }
    }
}