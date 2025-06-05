namespace GameLogEscritorio.Servicios.GameLogAPIGRPC.Respuesta
{
    public class RespuestaGRPC
    {
        public int codigo {  get; set; }

        public string? detalles { get; set; }

        public byte[]? datosBinario {  get; set; }   
    }
}
