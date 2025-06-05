using System.Text.RegularExpressions;

namespace GameLogEscritorio.Utilidades
{
    public static class Validador
    {
        private static Regex _correoRegex = new Regex(@"^[a-zA-Z0-9](?!.*[.-]{2})[a-zA-Z0-9.-]*[a-zA-Z0-9]@[a-zA-Z0-9](?!.*[.-]{2})[a-zA-Z0-9.-]*\.[a-zA-Z]{2,}$", RegexOptions.None, TimeSpan.FromMilliseconds(300));
        private static Regex _contraseniaRegex = new Regex(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[!@#$%^&*()_+\-=\[\]{};':""\\|,.<>\/?])[^ ]{8,}$", RegexOptions.None, TimeSpan.FromMilliseconds(300));
        private static Regex _nombresRegex = new Regex(@"^[a-zA-ZñÑáéíóúÁÉÍÓÚçÇ' -]+$", RegexOptions.None,TimeSpan.FromMilliseconds(300));
        private static Regex _fechaRegex = new Regex(@"^(19|20)\d{2}-(0[1-9]|1[0-2])-(0[1-9]|[12]\d|3[01])$", RegexOptions.None, TimeSpan.FromMilliseconds(1000));
        private static Regex _codigoVerificacionRegex = new Regex(@"^[0-9]{6}$", RegexOptions.None, TimeSpan.FromMilliseconds(300));
        private static Regex _soloLetrasYNumeros = new Regex(@"^[a-zA-Z0-9]{2,20}$", RegexOptions.None, TimeSpan.FromMilliseconds(300));
        private static readonly Regex _soloLetrasNumerosCaracteres = new Regex(@"^[a-zA-ZñÑáéíóúÁÉÍÓÚ0-9\s!&*()_+=\[\]{};:'"",.¿?`\\-]*$", RegexOptions.None, TimeSpan.FromMilliseconds(300));
        private static readonly Regex _soloDecimalesPositivos = new Regex(@"^\d{1,2}\.\d{1}$", RegexOptions.None, TimeSpan.FromMilliseconds(300));
        public static readonly Regex SoloRutas = new Regex(@"^(\/?[a-zA-Z0-9_-]+\/)*[a-zA-Z0-9_-]+\.[a-zA-Z0-9]{2,255}$", RegexOptions.None, TimeSpan.FromMilliseconds(300));


        public static bool ValidarPatronRegex(string datos, Regex regex)
        {
            bool esValido = false;
            try
            {
                esValido = regex.IsMatch(datos);
            }
            catch (RegexMatchTimeoutException)
            {
                esValido = false;
            }
            return esValido;
        }

        public static bool ValidarCorreo(string correo)
        {
            bool esValida = false;
            string correoLimpio = Regex.Replace(correo.Trim(), @"\s+", " ", RegexOptions.None, TimeSpan.FromMilliseconds(500));
            if (!string.IsNullOrWhiteSpace(correoLimpio) && ValidarPatronRegex(correoLimpio, _correoRegex))
            {
                esValida = true;
            }
            return esValida;
        }

        public static bool ValidarContrasenia(string contrasenia)
        {
            bool esValida = false;
            string contraseniaLimpia = Regex.Replace(contrasenia.Trim(), @"\s+", " ", RegexOptions.None, TimeSpan.FromMilliseconds(500));
            if (!string.IsNullOrWhiteSpace(contraseniaLimpia) && ValidarPatronRegex(contraseniaLimpia, _contraseniaRegex))
            {
                esValida = true;
            }
            return esValida;
        }

        public static bool ValidarSoloNombres(string nombre)
        {
            bool esValida = false;
            string nombreLimpio = Regex.Replace(nombre.Trim(), @"\s+", " ", RegexOptions.None, TimeSpan.FromMilliseconds(500));
            if (!string.IsNullOrWhiteSpace(nombreLimpio) && ValidarPatronRegex(nombreLimpio, _nombresRegex) && nombreLimpio.Length <=80 && nombreLimpio.Length >= 1)
            {
                esValida = true;
            }
            return esValida;
        }

        public static bool ValidarSegundoApellido(string segundoApellido)
        {
            bool esValida = false;
            if(!string.IsNullOrEmpty(segundoApellido))
            {
                string segundoApellidoLimpio = Regex.Replace(segundoApellido.Trim(), @"\s+", " ", RegexOptions.None, TimeSpan.FromMilliseconds(500));
                if(ValidarPatronRegex(segundoApellidoLimpio, _nombresRegex) && segundoApellidoLimpio.Length <= 80 && segundoApellidoLimpio.Length >= 2)
                {
                    esValida=true;
                }
            }
            else
            {
                esValida =true;
            }
            return esValida;
        }

        public static bool ValidarFecha(string fecha)
        {
            bool esValida = false;
            string fechaLimpia = Regex.Replace(fecha.Trim(), @"\s+", " ", RegexOptions.None, TimeSpan.FromMilliseconds(500));
            if (!string.IsNullOrWhiteSpace(fechaLimpia) && ValidarPatronRegex(fechaLimpia, _fechaRegex))
            {
                esValida = true;
            }
            return esValida;
        }

        public static bool ValidarCodigo(string codigo)
        {
            bool esValida = false;
            string codigoLimpio = Regex.Replace(codigo.Trim(), @"\s+", "", RegexOptions.None, TimeSpan.FromMilliseconds(500));
            if (!string.IsNullOrWhiteSpace(codigoLimpio) && ValidarPatronRegex(codigoLimpio, _codigoVerificacionRegex))
            {
                esValida = true;
            }
            return esValida;
        }

        public static bool ValidarNombreDeUsuario(string nombreDeUsuario)
        {
            bool esValida = false;
            string nombreDeUsuarioLimpio = Regex.Replace(nombreDeUsuario.Trim(), @"\s+", "", RegexOptions.None, TimeSpan.FromMilliseconds(500));
            if (!string.IsNullOrWhiteSpace(nombreDeUsuarioLimpio) && ValidarPatronRegex(nombreDeUsuarioLimpio, _soloLetrasYNumeros))
            {
                esValida = true;
            }
            return esValida;
        }

        public static bool ValidarCalificacion(string calificacion)
        {
            bool esValida = false;
            string calificacionLimpia = Regex.Replace(calificacion.Trim(), @"\s+", "", RegexOptions.None, TimeSpan.FromMilliseconds(500));
            if (!string.IsNullOrWhiteSpace(calificacionLimpia) && ValidarPatronRegex(calificacionLimpia, _soloDecimalesPositivos))
            {
                esValida = true;
            }
            return esValida;
        }

        public static bool ValidarRutas(string ruta)
        {
            bool esValida = false;
            string rutaLimpia = Regex.Replace(ruta.Trim(), @"\s+", "", RegexOptions.None, TimeSpan.FromMilliseconds(500));
            if (!string.IsNullOrWhiteSpace(rutaLimpia) && ValidarPatronRegex(rutaLimpia, SoloRutas))
            {
                esValida = true;
            }
            return esValida;
        }

        public static bool ValidarDescripcion(string descripcion)
        {
            bool esValida = false;
            if (!string.IsNullOrEmpty(descripcion))
            {
                string descripcionLimpio = Regex.Replace(descripcion.Trim(), @"\s+", "", RegexOptions.None, TimeSpan.FromMilliseconds(500));
                if (ValidarPatronRegex(descripcion, _soloLetrasNumerosCaracteres) && descripcionLimpio.Length>=1 && descripcionLimpio.Length<=200)
                {
                    esValida = true;
                }
            }
            else
            {
                esValida = true;
            }
            return esValida;
        }

        public static bool ValidarOpinion(string opinion)
        {
            bool esValida = false;
            if (!string.IsNullOrEmpty(opinion))
            {
                string opinionLimpia = Regex.Replace(opinion.Trim(), @"\s+", "", RegexOptions.None, TimeSpan.FromMilliseconds(500));
                if (ValidarPatronRegex(opinionLimpia, _soloLetrasNumerosCaracteres) && opinionLimpia.Length >= 1 && opinionLimpia.Length <= 200)
                {
                    esValida = true;
                }
            }
            else
            {
                esValida = true;
            }
            return esValida;
        }
    }
}
