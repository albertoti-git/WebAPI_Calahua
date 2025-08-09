using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApiRESTv1.Models;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Microsoft.AspNetCore.JsonPatch;
using System.Dynamic;
using Newtonsoft.Json.Converters;
using SAPbobsCOM;

namespace WebApiRESTv1.Controllers
{
	[Authorize]
	public class BusinessPartnerController : ApiController
    {
        string strConection = ConfigurationManager.AppSettings.Get("bdcon");

        [Route("BusinessPartners")]
		//	public IHttpActionResult GetSocios(PageParameter pageParameter)
		public IHttpActionResult GetSocios(int PageNumber, int PageSize)
		{
			DataTable dt = new DataTable();
			DataTable dtAdress = new DataTable();
			DataTable dtContacts = new DataTable();
			dynamic json = null;
			int iSkip = 0;
			//iSkip = (pageParameter.PageNumber - 1) * pageParameter.PageSize;
			iSkip = (PageNumber - 1) * PageSize;
			using (SqlConnection connection = new SqlConnection(strConection))
			{
				String sql = string.Format("Sp_AYB_WebAPI {0}, '{1}', '{2}', '{3}'", 2, "", iSkip, PageSize);
				SqlCommand cmd = new SqlCommand();
				SqlDataAdapter sqlDA;
				connection.Open();
				cmd.CommandText = sql;
				cmd.CommandType = CommandType.Text;
				cmd.Connection = connection;
				sqlDA = new SqlDataAdapter(cmd);
				sqlDA.Fill(dt);

			}
			int ColCount = dt.Columns.Count;
			int RowCount = dt.Rows.Count;
			string ITEMS = string.Empty;
			if (RowCount > 0)
			{
				ITEMS += "[";
				for (int j = 0; j < RowCount; j++) 
				{
					string ITEM = string.Empty;
					ITEM = "{";
					string ITEMVALUE = string.Empty;
					for (int i = 0; i < ColCount - 1; i++)
					{
						string ColName = dt.Columns[i].ColumnName;
						ITEMVALUE = dt.Rows[j][i].ToString();
						Type tipoDato = dt.Rows[j][i].GetType();
						if (tipoDato.Name == "String")
						{
							ITEM += "\"" + ColName + "\"" + " : " + "\"" + ITEMVALUE + "\"";
						}
						else
						{
							ITEM += "\"" + ColName + "\"" + " : " + ITEMVALUE;
						}

						ITEM += ",";

						if ( ColName == "CardCode" ) 
						{
							dtAdress.Rows.Clear();

							using (SqlConnection connection = new SqlConnection(strConection))
							{
								String sql = string.Format("Sp_AYB_WebAPI {0}, '{1}' ", 3, ITEMVALUE);
								SqlCommand cmd = new SqlCommand();
								SqlDataAdapter sqlDA;
								connection.Open();
								cmd.CommandText = sql;
								cmd.CommandType = CommandType.Text;
								cmd.Connection = connection;
								sqlDA = new SqlDataAdapter(cmd);
								sqlDA.Fill(dtAdress);

							}
							int ColCountAdr = dtAdress.Columns.Count;
							int RowCountAdr = dtAdress.Rows.Count;
							string ITEMSAdr = string.Empty;

							if (RowCountAdr > 0)
							{
								ITEMSAdr += " \"Addresses\" :[";
								for (int ja = 0; ja < RowCountAdr; ja++)
								{
									string ITEMAdr = string.Empty;
									ITEMAdr = "{";
									string ITEMVALUEAdr = string.Empty;
									for (int ia = 0; ia < ColCountAdr - 1; ia++)
									{
										string ColNameAdr = dtAdress.Columns[ia].ColumnName;
										ITEMVALUEAdr = dtAdress.Rows[ja][ia].ToString();
										Type tipoDatoAdr = dtAdress.Rows[ja][ia].GetType();
										
										if (tipoDatoAdr.Name == "String" || tipoDatoAdr.Name == "DateTime")
										{
											ITEMAdr += "\"" + ColNameAdr + "\"" + " : " + "\"" + ITEMVALUEAdr + "\"";
										}
										else
										{

											if (tipoDatoAdr.Name == "DBNull")
											{
												ITEMAdr += "\"" + ColNameAdr + "\"" + " : \"\"";
											}
											else
											{
												ITEMAdr += "\"" + ColNameAdr + "\"" + " : " + ITEMVALUEAdr;
											}

										}

										ITEMAdr += ",";

									}
									if (ITEMVALUEAdr.Length > 0)
									{
										ITEMAdr = ITEMAdr.Substring(0, ITEMAdr.Length - 1);
									}

									ITEMAdr += "},";
									ITEMSAdr += ITEMAdr;

								}
								ITEMSAdr = ITEMSAdr.Substring(0, ITEMSAdr.Length - 1);
								ITEMSAdr += "]";

								ITEM += ITEMSAdr;
								
							}
							else {
								ITEM += " \"Addresses\" :null";
							}
							ITEM += ",";
							dtContacts.Rows.Clear();
							using (SqlConnection connection = new SqlConnection(strConection))
							{
								String sql = string.Format("Sp_AYB_WebAPI {0}, '{1}' ", 4, ITEMVALUE);
								SqlCommand cmd = new SqlCommand();
								SqlDataAdapter sqlDA;
								connection.Open();
								cmd.CommandText = sql;
								cmd.CommandType = CommandType.Text;
								cmd.Connection = connection;
								sqlDA = new SqlDataAdapter(cmd);
								sqlDA.Fill(dtContacts);

							}

							int ColCountCnt = dtContacts.Columns.Count;
							int RowCountCnt = dtContacts.Rows.Count;
							string ITEMSCnt = string.Empty;

							/////
							if (RowCountCnt > 0)
							{
								ITEMSCnt += " \"ContactEmployees\" :[";
								for (int ja = 0; ja < RowCountCnt; ja++)
								{
									string ITEMCnt = string.Empty;
									ITEMCnt = "{";
									string ITEMVALUEACnt = string.Empty;
									for (int ia = 0; ia < ColCountCnt - 1; ia++)
									{
										string ColNameCnt = dtContacts.Columns[ia].ColumnName;
										ITEMVALUEACnt = dtContacts.Rows[ja][ia].ToString();
										Type tipoDatoCnt = dtContacts.Rows[ja][ia].GetType();
										if (tipoDatoCnt.Name == "String" || tipoDatoCnt.Name == "DateTime")
										{
											ITEMCnt += "\"" + ColNameCnt + "\"" + " : " + "\"" + ITEMVALUEACnt + "\"";
										}
										else
										{
											if (tipoDatoCnt.Name == "DBNull")
											{
												ITEMCnt += "\"" + ColNameCnt + "\"" + " : \"\"";
											}
											else
											{
												ITEMCnt += "\"" + ColNameCnt + "\"" + " : " + ITEMVALUEACnt;
											}
										}

										ITEMCnt += ",";

									}
									if (ITEMVALUEACnt.Length > 0)
									{
										ITEMCnt = ITEMCnt.Substring(0, ITEMCnt.Length - 1);
									}

									ITEMCnt += "},";
									ITEMSCnt += ITEMCnt;

								}
								ITEMSCnt = ITEMSCnt.Substring(0, ITEMSCnt.Length - 1);
								ITEMSCnt += "]";

								ITEM += ITEMSCnt;

							}
							else
							{
								ITEM += " \"ContactEmployees\" :null";
							}
							ITEM += ",";


							////

						}


					}
					if (ITEMVALUE.Length > 0)
					{
						ITEM = ITEM.Substring(0, ITEM.Length - 1);
					}									

					ITEM += "},";
					ITEMS += ITEM;
				}
				ITEMS = ITEMS.Substring(0, ITEMS.Length - 1);
				ITEMS += "]";

				json = JsonConvert.DeserializeObject(ITEMS);
				return Ok(json);
			}
			else
			{
				//BusinessPartner oB = new BusinessPartner();
				ITEMS = "{}";

				json = JsonConvert.DeserializeObject(ITEMS);
				return Ok(json);

			}
		
		}


		public HttpResponseMessage post(BusinessPartner bp)
        {
            string str_sp = ConfigurationManager.AppSettings.Get("spPOSTBusinessPartener");

			//string SQL = string.Format("{0}, '{1}','{2}',{3},'{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}',{12},{13},{14},'{15}',{16},'{17}',{18},'{19}','{20}',{21},'{22}','{23}','{24}','{25}','{26}','{27}','{28}','{29}','{30}','{31}','{32}',{33},{34},{35},{36},'{37}','{38}',{39},'{40}','{41}','{42}','{43}',{44},'{45}','{46}',", str_sp,
			//    bp.CardCode,bp.CardType,bp.CardName,bp.GroupCode,bp.Address,bp.ZipCode,bp.MailAddress,bp.MailZipCode,
			//    bp.Phone1,bp.Phone2,bp.Fax,bp.ContactPerson,bp.PayTermsGrpCode,bp.CreditLimit,bp.DiscountPercent,bp.FederalTaxID,
			//    bp.PriceListNum,bp.FreeText,bp.SalesPersonCode,bp.Currency,bp.Cellular,bp.AvarageLate,bp.City,bp.County,
			//    bp.Country,bp.MailCity,bp.MailCounty,bp.MailCountry,bp.EmailAddress,bp.Picture,bp.AdditionalID,bp.FatherCard,
			//    bp.CardForeignName,bp.CurrentAccountBalance,bp.OpenDeliveryNotesBalance,bp.OpenOrdersBalance,bp.Valid,bp.ValidFrom.ToString("yyyyMMdd"),
			//    bp.ValidTo.ToString("yyyyMMdd"),bp.Frozen,bp.FrozenFrom.ToString("yyyyMMdd"),bp.FrozenTo.ToString("yyyyMMdd"),bp.Block,bp.ProjectCode,bp.Series,
			//    bp.GlobalLocationNumber,bp.UnifiedFederalTaxID, bp.UsoCFDI);

			//using (SqlConnection connection = new SqlConnection(strConection))
			//{
			//    SqlCommand cmd = new SqlCommand();
			//    connection.Open();
			//    cmd.CommandText = SQL;
			//    cmd.CommandType = CommandType.Text;
			//    cmd.Connection = connection;
			//    int Bid = Convert.ToInt32(cmd.ExecuteScalar());

			//    string sql2 = string.Format("{0} {1},'{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}',{14},'{15}','{16}','{17}','{18}','{19}','{20}',{21},'{22}','{23}','{24}',",
			//    "insertacontact_employs ",Bid, bp.ContactEmployees.Address, bp.ContactEmployees.Phone1, bp.ContactEmployees.Phone2, bp.ContactEmployees.MobilePhone, bp.ContactEmployees.Fax, bp.ContactEmployees.E_Mail,
			//    bp.ContactEmployees.Pager, bp.ContactEmployees.Remarks1, bp.ContactEmployees.Remarks2, bp.ContactEmployees.Password, bp.ContactEmployees.Name, bp.ContactEmployees.InternalCode, bp.ContactEmployees.PlaceOfBirth,
			//    bp.ContactEmployees.DateOfBirth, bp.ContactEmployees.Profession, bp.ContactEmployees.CardCode, bp.ContactEmployees.Title, bp.ContactEmployees.CityOfBirth, bp.ContactEmployees.Active, bp.ContactEmployees.FirstName, bp.ContactEmployees.MiddleName, bp.ContactEmployees.LastName);
			//    cmd.CommandText = sql2;
			//    cmd.ExecuteNonQuery();
			//}

			ConexionSAP conexionSAP = ConexionSAP.GetInstance;
			BusinessPartners oSocios = (BusinessPartners)(dynamic)conexionSAP.CompanySBO.GetBusinessObject(BoObjectTypes.oBusinessPartners);
			Recordset recordset = (Recordset)(dynamic)conexionSAP.CompanySBO.GetBusinessObject(BoObjectTypes.BoRecordset);
			if (!oSocios.GetByKey(bp.CardCode))
			{
				if (bp.Series <= 0)
				{
					oSocios.CardCode = bp.CardCode;
				}
				else
				{
					oSocios.Series = bp.Series;
				}
				oSocios.CardName = bp.CardName;
				oSocios.CardType = BoCardTypes.cCustomer;
				oSocios.CardForeignName = bp.CardForeignName;
				oSocios.FederalTaxID = bp.FederalTaxID;
				if (bp.GroupCode > 0)
				{
					oSocios.GroupCode = bp.GroupCode;
				}
				else
				{
					oSocios.GroupCode = 103;
				}
				if (bp.PriceListNum > 0)
				{
					oSocios.PriceListNum = bp.PriceListNum;
				}
				oSocios.CreditLimit = bp.CreditLimit;
				oSocios.Currency = bp.Currency;
				oSocios.Phone1 = bp.Phone1;
				oSocios.Phone2 = bp.Phone2;
				oSocios.Cellular = bp.Cellular;
				oSocios.EmailAddress = bp.EmailAddress;
				oSocios.AdditionalID = bp.AdditionalID;
				oSocios.MailAddress = bp.MailAddress;
				oSocios.Address = bp.Address;
				oSocios.City = bp.City;
				oSocios.County = bp.County;
				oSocios.Country = bp.Country;
				oSocios.ZipCode = bp.ZipCode;
				oSocios.Block = bp.Block;
				oSocios.UnifiedFederalTaxID = bp.UnifiedFederalTaxID;
				oSocios.GlobalLocationNumber = bp.GlobalLocationNumber;
				oSocios.FatherCard = bp.FatherCard;
				oSocios.MailZipCode = bp.MailZipCode;
				oSocios.MailCity = bp.MailCity;
				oSocios.MailCounty = bp.MailCounty;
				oSocios.MailCountry = bp.MailCountry;
				oSocios.ProjectCode = bp.ProjectCode;
				oSocios.Fax = bp.Fax;
				oSocios.FreeText = oSocios.FreeText;
				if (bp.UserFields != null && bp.UserFields.Count > 0)
				{
					foreach (string sCampo in bp.UserFields.Keys)
					{
						oSocios.UserFields.Fields.Item(sCampo).Value = bp.UserFields[sCampo];
					}
				}
				if (bp.Addresses != null && bp.Addresses.Count() > 0)
				{
					for (int iIndex2 = 0; iIndex2 < oSocios.Addresses.Count; iIndex2++)
					{
						if (iIndex2 > 0)
						{
							oSocios.Addresses.Add();
						}
						BPAddress oDir2 = bp.Addresses[iIndex2];
						oSocios.Addresses.AddressName = oDir2.AddressName;
						oSocios.Addresses.AddressType = ((!(oDir2.AddressType == "bo_ShipTo")) ? BoAddressType.bo_BillTo : BoAddressType.bo_ShipTo);
						oSocios.Addresses.Street = oDir2.Street;
						oSocios.Addresses.Block = oDir2.Block;
						oSocios.Addresses.ZipCode = oDir2.ZipCode;
						oSocios.Addresses.City = oDir2.City;
						oSocios.Addresses.County = oDir2.County;
						oSocios.Addresses.Country = oDir2.Country;
						oSocios.Addresses.State = oDir2.State;
						oSocios.Addresses.FederalTaxID = oDir2.FederalTaxID;
						oSocios.Addresses.TaxCode = oDir2.TaxCode;
						oSocios.Addresses.BuildingFloorRoom = oDir2.BuildingFloorRoom;
						oSocios.Addresses.StreetNo = oDir2.StreetNo;
						oSocios.Addresses.GlobalLocationNumber = oDir2.GlobalLocationNumber;
					}
				}
				if (oSocios.Add() != 0)
				{
					HttpResponseMessage response2 = base.Request.CreateResponse(HttpStatusCode.BadRequest, bp);
					response2.ReasonPhrase = conexionSAP.CompanySBO.GetLastErrorDescription();
					return response2;
				}
				string sCardCode2 = (bp.CardCode = conexionSAP.CompanySBO.GetNewObjectKey());
				bp.Valid = true;
				bp = Get_reponse(sCardCode2);
				return base.Request.CreateResponse(HttpStatusCode.Created, bp);
			}
			oSocios.CardName = bp.CardName;
			oSocios.CardType = ((!(bp.CardType == "cCustomer")) ? BoCardTypes.cSupplier : BoCardTypes.cCustomer);
			oSocios.CardForeignName = bp.CardForeignName;
			oSocios.FederalTaxID = bp.FederalTaxID;
			oSocios.GroupCode = bp.GroupCode;
			if (bp.PriceListNum > 0)
			{
				oSocios.PriceListNum = bp.PriceListNum;
			}
			oSocios.CreditLimit = bp.CreditLimit;
			oSocios.Currency = bp.Currency;
			oSocios.Phone1 = bp.Phone1;
			oSocios.Phone2 = bp.Phone2;
			oSocios.Cellular = bp.Cellular;
			oSocios.EmailAddress = bp.EmailAddress;
			oSocios.AdditionalID = bp.AdditionalID;
			oSocios.MailAddress = bp.MailAddress;
			oSocios.Address = bp.Address;
			oSocios.City = bp.City;
			oSocios.County = bp.County;
			oSocios.Country = bp.Country;
			oSocios.ZipCode = bp.ZipCode;
			oSocios.Block = bp.Block;
			oSocios.UnifiedFederalTaxID = bp.UnifiedFederalTaxID;
			oSocios.GlobalLocationNumber = bp.GlobalLocationNumber;
			oSocios.FatherCard = bp.FatherCard;
			oSocios.MailZipCode = bp.MailZipCode;
			oSocios.MailCity = bp.MailCity;
			oSocios.MailCounty = bp.MailCounty;
			oSocios.MailCountry = bp.MailCountry;
			oSocios.ProjectCode = bp.ProjectCode;
			oSocios.Fax = bp.Fax;
			oSocios.FreeText = oSocios.FreeText;
			if (bp.UserFields != null && bp.UserFields.Count > 0)
			{
				foreach (string sCampo2 in bp.UserFields.Keys)
				{
					oSocios.UserFields.Fields.Item(sCampo2).Value = bp.UserFields[sCampo2];
				}
			}
			if (bp.Addresses != null && bp.Addresses.Count() > 0)
			{
				for (int iIndex3 = 0; iIndex3 < oSocios.Addresses.Count; iIndex3++)
				{
					if (iIndex3 > 0)
					{
						oSocios.Addresses.Add();
					}
					BPAddress oDir = bp.Addresses[iIndex3];
					oSocios.Addresses.AddressName = oDir.AddressName;
					oSocios.Addresses.AddressType = ((!(oDir.AddressType == "bo_ShipTo")) ? BoAddressType.bo_BillTo : BoAddressType.bo_ShipTo);
					oSocios.Addresses.Street = oDir.Street;
					oSocios.Addresses.Block = oDir.Block;
					oSocios.Addresses.ZipCode = oDir.ZipCode;
					oSocios.Addresses.City = oDir.City;
					oSocios.Addresses.County = oDir.County;
					oSocios.Addresses.Country = oDir.Country;
					oSocios.Addresses.State = oDir.State;
					oSocios.Addresses.FederalTaxID = oDir.FederalTaxID;
					oSocios.Addresses.TaxCode = oDir.TaxCode;
					oSocios.Addresses.BuildingFloorRoom = oDir.BuildingFloorRoom;
					oSocios.Addresses.StreetNo = oDir.StreetNo;
					oSocios.Addresses.GlobalLocationNumber = oDir.GlobalLocationNumber;
				}
			}
			if (oSocios.Update() != 0)
			{
				HttpResponseMessage response = base.Request.CreateResponse(HttpStatusCode.BadRequest, bp);
				response.ReasonPhrase = conexionSAP.CompanySBO.GetLastErrorDescription();
				return response;
			}
			string sCardCode = (bp.CardCode = conexionSAP.CompanySBO.GetNewObjectKey());
			bp.Valid = true;
			bp = Get_reponse(sCardCode);
			return base.Request.CreateResponse(HttpStatusCode.Accepted, bp);


			//  return Ok();
		}

		[Route("BusinessPartner")]
		public BusinessPartner Get_reponse(string CardCode)
		{
			BusinessPartner businessPartner = new BusinessPartner();
			try
			{
				ConexionSAP conexionSAP = ConexionSAP.GetInstance;
				BusinessPartners oSocios = (BusinessPartners)(dynamic)conexionSAP.CompanySBO.GetBusinessObject(BoObjectTypes.oBusinessPartners);
				Recordset recordset = (Recordset)(dynamic)conexionSAP.CompanySBO.GetBusinessObject(BoObjectTypes.BoRecordset);
				recordset.DoQuery("SELECT  \"CardCode\" FROM OCRD WHERE \"CardCode\" = '" + CardCode + "'");
				oSocios.Browser.Recordset = recordset;
				while (!oSocios.Browser.EoF)
				{
					businessPartner.CardCode = oSocios.CardCode;
					businessPartner.CardName = oSocios.CardName;
					businessPartner.CardForeignName = oSocios.CardForeignName;
					businessPartner.FederalTaxID = oSocios.FederalTaxID;
					businessPartner.GroupCode = oSocios.GroupCode;
					businessPartner.CardType = oSocios.CardType.ToString();
					businessPartner.PriceListNum = oSocios.PriceListNum;
					businessPartner.CreditLimit = oSocios.CreditLimit;
					businessPartner.Currency = oSocios.Currency;
					businessPartner.OpenDeliveryNotesBalance = oSocios.OpenDeliveryNotesBalance;
					businessPartner.OpenOrdersBalance = oSocios.OpenOrdersBalance;
					businessPartner.Phone1 = oSocios.Phone1;
					businessPartner.Phone2 = oSocios.Phone2;
					businessPartner.Cellular = oSocios.Cellular;
					businessPartner.EmailAddress = oSocios.EmailAddress;
					businessPartner.AdditionalID = oSocios.AdditionalID;
					businessPartner.UsoCFDI = ((dynamic)oSocios.UserFields.Fields.Item("U_B1SYS_MainUsage").Value).ToString();
					businessPartner.Valid = ((oSocios.Valid == BoYesNoEnum.tYES) ? true : false);
					businessPartner.MailAddress = oSocios.MailAddress;
					businessPartner.Address = oSocios.Address;
					businessPartner.City = oSocios.City;
					businessPartner.County = oSocios.County;
					businessPartner.Country = oSocios.Country;
					businessPartner.ZipCode = oSocios.ZipCode;
					businessPartner.Block = oSocios.Block;
					businessPartner.UnifiedFederalTaxID = oSocios.UnifiedFederalTaxID;
					businessPartner.GlobalLocationNumber = oSocios.GlobalLocationNumber;
					businessPartner.FatherCard = oSocios.FatherCard;
					businessPartner.MailZipCode = oSocios.MailZipCode;
					businessPartner.MailCity = oSocios.MailCity;
					businessPartner.MailCounty = oSocios.MailCounty;
					businessPartner.MailCountry = oSocios.MailCountry;
					businessPartner.ProjectCode = oSocios.ProjectCode;
					businessPartner.Fax = oSocios.Fax;
					businessPartner.FreeText = oSocios.FreeText;
					businessPartner.UserFields = new Dictionary<string, string>();
					businessPartner.Addresses = new BPAddress[oSocios.Addresses.Count];
					for (int iIndex2 = 0; iIndex2 < oSocios.Addresses.Count; iIndex2++)
					{
						oSocios.Addresses.SetCurrentLine(iIndex2);
						businessPartner.Addresses[iIndex2] = new BPAddress();
						businessPartner.Addresses[iIndex2].AddressName = oSocios.Addresses.AddressName;
						businessPartner.Addresses[iIndex2].AddressType = oSocios.Addresses.AddressType.ToString();
						businessPartner.Addresses[iIndex2].Street = oSocios.Addresses.Street;
						businessPartner.Addresses[iIndex2].Block = oSocios.Addresses.Block;
						businessPartner.Addresses[iIndex2].ZipCode = oSocios.Addresses.ZipCode;
						businessPartner.Addresses[iIndex2].City = oSocios.Addresses.City;
						businessPartner.Addresses[iIndex2].County = oSocios.Addresses.County;
						businessPartner.Addresses[iIndex2].Country = oSocios.Addresses.Country;
						businessPartner.Addresses[iIndex2].State = oSocios.Addresses.State;
						businessPartner.Addresses[iIndex2].FederalTaxID = oSocios.Addresses.FederalTaxID;
						businessPartner.Addresses[iIndex2].TaxCode = oSocios.Addresses.TaxCode;
						businessPartner.Addresses[iIndex2].BuildingFloorRoom = oSocios.Addresses.BuildingFloorRoom;
						businessPartner.Addresses[iIndex2].StreetNo = oSocios.Addresses.StreetNo;
						businessPartner.Addresses[iIndex2].GlobalLocationNumber = oSocios.Addresses.GlobalLocationNumber;
					}
					oSocios.Browser.MoveNext();
				}
			}
			catch (Exception ex )
			{
			}
			return businessPartner;
		}


    }
}
