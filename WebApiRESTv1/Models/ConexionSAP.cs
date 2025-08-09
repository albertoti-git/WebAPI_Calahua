using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SAPbobsCOM;

namespace WebApiRESTv1.Models
{
	public class ConexionSAP
	{
		private static ConexionSAP instance = null;

		public static ConexionSAP GetInstance
		{
			get
			{
				if (instance == null)
				{
					instance = new ConexionSAP();
				}
				else if (instance.CompanySBO == null && !string.IsNullOrEmpty(instance.Servidor) && !string.IsNullOrEmpty(instance.TipoServidor) && !string.IsNullOrEmpty(instance.Catalogo) && !string.IsNullOrEmpty(instance.UsuarioSAP) && !string.IsNullOrEmpty(instance.ContrasenaSBO))
				{
					instance.Conectar();
				}
				return instance;
			}
		}

		public string Servidor { get; set; }

		public string SLDServer { get; set; }

		public string TipoServidor { get; set; }

		public string Usuario { get; set; }

		public string UsuarioSAP { get; set; }

		public bool EsDeConfianza { get; set; }

		public string Catalogo { get; set; }

		public string Contraseña { get; set; }

		public string ContrasenaSBO { get; set; }

		public Company CompanySBO { get; set; }

		private ConexionSAP()
		{
			try
			{
				CompanySBO = new SAPbobsCOM.Company();
			}
			catch (Exception)
			{
			}
		}

		public bool Conectar()
		{
			try
			{
				CompanySBO = new Company();
				switch (TipoServidor)
				{
				
					case "dst_MSSQL2014":
						CompanySBO.Server = Servidor;
						CompanySBO.DbServerType = BoDataServerTypes.dst_MSSQL2014;
						break;
					case "dst_MSSQL2016":
						CompanySBO.Server = Servidor;
						CompanySBO.DbServerType = BoDataServerTypes.dst_MSSQL2016;
						break;
					case "dst_MSSQL2017":
						CompanySBO.Server = Servidor;
						CompanySBO.DbServerType = BoDataServerTypes.dst_MSSQL2017;
						break;
					//case "dst_MSSQL2019":
					//	CompanySBO.Server = Servidor;
					//	CompanySBO.DbServerType = BoDataServerTypes.dst_MSSQL2019;
					//	break;
					case "dst_HANADB":
						CompanySBO.Server = Servidor + ":30013";
						CompanySBO.DbServerType = BoDataServerTypes.dst_HANADB;
						break;
				}
				CompanySBO.UseTrusted = false;
				CompanySBO.DbUserName = Usuario;
				CompanySBO.DbPassword = Contraseña;
				CompanySBO.CompanyDB = Catalogo;
				CompanySBO.UserName = UsuarioSAP;
				CompanySBO.Password = ContrasenaSBO;
				CompanySBO.language = BoSuppLangs.ln_English;
				if (CompanySBO.Connect() != 0)
				{
					Console.Write(CompanySBO.GetLastErrorDescription());
					return false;
				}
				return true;
			}
			catch (Exception)
			{
				return false;
			}
		}



	}

}
