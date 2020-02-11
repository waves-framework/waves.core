namespace Fluid.Core.EventArgs
{
    public class DataReceivedEventArgs
    {
        /// <summary>
        /// Новый экземпляр аргументов приема данных.
        /// </summary>
        /// <param name="sender">Отправитель.</param>
        /// <param name="index">Индекс отправителя.</param>
        /// <param name="data"></param>
        public DataReceivedEventArgs(object sender, int index, object data)
        {
            Sender = sender;
            SenderIndex = index;
            Data = data;
        }

        /// <summary>
        /// Отправитель.
        /// </summary>
        public object Sender { get; private set; }

        /// <summary>
        /// Индекс отправителя.
        /// </summary>
        public int SenderIndex { get; private set; }

        /// <summary>
        /// Передаваемые данные.
        /// </summary>
        public object Data { get; private set; }
    }
}