using System.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Models;

namespace backend.Controllers;

[ApiController]
[Route("[controller]")]
public class PrimaController : ControllerBase
{
	private string _database;

	public PrimaController(IConfiguration config)
	{
		_database = config["ConnectionStrings:SQL"] ?? "";
	}

	[HttpGet]
	[Route("/[controller]/{codigoCliente}")]
	public List<PrimaModel> Get(string codigoCliente)
	{
		List<PrimaModel> items = new List<PrimaModel>();

		string queryString = "consulta1";

		using (SqlConnection connection = new SqlConnection(_database))
		{
			SqlCommand command = new SqlCommand(queryString, connection);
			command.CommandType = CommandType.StoredProcedure;
			command.Parameters.AddWithValue("@codigo_cliente", codigoCliente);

			connection.Open();

			using (SqlDataReader reader = command.ExecuteReader())
			{
				while (reader.Read())
				{
					PrimaModel item = new PrimaModel
					{
						CodigoCliente = reader.GetString(reader.GetOrdinal("Codigo_cliente")),
						CodigoAsegurado = reader.GetString(reader.GetOrdinal("Codigo_asegurado")),
						CodigoProducto = reader.GetString(reader.GetOrdinal("Codigo_producto")),
						LiquidacionPrima = reader.GetString(reader.GetOrdinal("Liquidacion_prima")),
						NumeroDocumento = reader.GetString(reader.GetOrdinal("Numero_documento")),
						NumeroPoliza = reader.GetString(reader.GetOrdinal("Numero_poliza")),
						TipoDocumento = reader.GetString(reader.GetOrdinal("Tipo_documento")),
						TipoRegistro = reader.GetString(reader.GetOrdinal("Tipo_registro")),
					};

					items.Add(item);
				}
			}
		}

		return items;
	}
}
