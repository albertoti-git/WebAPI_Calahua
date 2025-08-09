using Newtonsoft.Json;
using System;

namespace WebApiRESTv1.Util
{
    public class Response<T>
    {
        public Response()
        {
            _isSuccess = false;
            ErrorDetails = new ErrorDetails();
        }

        public string Mensaje { get; set; }

        [JsonIgnore]
        public object MensajeJson { get; set; }

        public T Dato { get; set; }

        public string MensajeDev { get; set; }

        public ErrorDetails ErrorDetails { get; set; }

        private bool _isSuccess;
        public bool IsSuccess
        {
            get => _isSuccess;
            set
            {
                _isSuccess = value;
                if (value)
                    ErrorDetails = null;
            }
        }

        public Response<T> Ok(T dato)
        {
            this.Mensaje = Message.correcto;
            this.Dato = dato;
            this.IsSuccess = true;
            return this;
        }
        public Response<T> Falla(string mensaje)
        {
            return new Response<T>
            {
                IsSuccess = false,
                Mensaje = mensaje
            };
        }
    }

    public class Response
    {
        public string Mensaje { get; set; }

        [JsonIgnore]
        public object MensajeJson { get; set; }

        public string MensajeDev { get; set; }

        public bool IsSuccess { get; set; }

        public Response Ok()
        {
            this.Mensaje = Message.correcto;
            this.IsSuccess = true;
            return this;
        }
    }

    public class ErrorDetails
    {
        public string Codigo { get; set; }
        public string Descripcion { get; set; }
        public string StackTrace { get; set; }
    }

    public static class Message
    {
        public const string correcto = "Operación Completada, ejecución exitosa";
    }

}
