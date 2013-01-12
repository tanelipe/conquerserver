namespace NetworkLibrary
{
    public delegate void NetworkEvent<T, T2>(T Sender, T2 Parameter);
    public delegate void NetworkEvent<T, T2, T3>(T Sender, T2 Parameter, T3 Extension);
}
