using System.Collections.Generic;
using System.IO;
using System.Linq;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using Microsoft.AspNetCore.Mvc;
using MiPrimerProyectoEnNetCore5.Clases;
using MiPrimerProyectoEnNetCore5.Filters;
using OfficeOpenXml;
using Syncfusion.DocIO;
using Syncfusion.DocIO.DLS;
using Syncfusion.Drawing;
using cm = System.ComponentModel;
namespace MiPrimerProyectoEnNetCore5.Controllers
{
	[ServiceFilter(typeof(Seguridad))]
	public class BaseController : Controller
	{
		public byte[] exportarWordDatos<T>(string[] nombrePropiedades, List<T> lista)
		{
			using (MemoryStream ms = new MemoryStream())
			{
				using (WordDocument wd = new WordDocument())
				{
					WSection section = wd.AddSection() as WSection;
					section.PageSetup.Margins.All = 72;
					section.PageSetup.PageSize = new Syncfusion.Drawing.SizeF(612, 792);
					IWParagraph paragraph = section.AddParagraph();
					paragraph.ApplyStyle("Normal");
					paragraph.ParagraphFormat.HorizontalAlignment = HorizontalAlignment.Center;
					WTextRange textRange = paragraph.AppendText("Reporte Word") as WTextRange;
					textRange.CharacterFormat.FontSize = 20f;
					textRange.CharacterFormat.FontName = "Calibri";
					textRange.CharacterFormat.TextColor = Syncfusion.Drawing.Color.Blue;

					if (nombrePropiedades != null && nombrePropiedades.Length > 0)
					{
						IWTable table = section.AddTable();
						table.ResetCells(lista.Count + 1, nombrePropiedades.Length);

						Dictionary<string, string> diccionario = cm.TypeDescriptor.GetProperties(typeof(T)).Cast<cm.PropertyDescriptor>()
							.ToDictionary(p => p.Name, p => p.DisplayName);
						//Se añade la cabecera
						for (int i = 0; i < nombrePropiedades.Length; i++)
						{
							table[0, i].AddParagraph().AppendText(diccionario[nombrePropiedades[i]]);
						}

						int fila = 1;
						int col = 0;

						if (lista != null)
						{
							foreach (T item in lista)
							{
								col = 0;
								foreach (string propiedad in nombrePropiedades)
								{
									table[fila, col++].AddParagraph().AppendText((item.GetType().GetProperty(propiedad).GetValue(item)?.ToString()==null?"": item.GetType().GetProperty(propiedad).GetValue(item)?.ToString()));
								}
								fila++;
							}
						}
					}

					wd.Save(ms, FormatType.Docx);
					return ms.ToArray();
				}
			}
		}

		public byte[] exportarPdfDatos<T>(string[] nombrePropiedades, List<T> lista)
		{
			using (MemoryStream ms = new MemoryStream())
			{
				PdfWriter pdfWriter = new PdfWriter(ms);
				using (var pdfDoc = new PdfDocument(pdfWriter))
				{
					Document document = new Document(pdfDoc);
					Paragraph c1 = new Paragraph("Reporte en PDF");
					c1.SetFontSize(20);
					c1.SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER);
					document.Add(c1);

					if (nombrePropiedades != null && nombrePropiedades.Length > 0)
					{

						Table table = new Table(nombrePropiedades.Length);
						Cell celda;

						Dictionary<string, string> diccionario = cm.TypeDescriptor.GetProperties(typeof(T)).Cast<cm.PropertyDescriptor>()
							.ToDictionary(p => p.Name, p => p.DisplayName);

						//Se añade la cabecera
						for (int i = 0; i < nombrePropiedades.Length; i++)
						{
							celda = new Cell();
							celda.Add(new Paragraph(diccionario[nombrePropiedades[i]]));
							table.AddHeaderCell(celda);
						}

						//Se añaden los datos
						if (lista != null)
						{
							foreach (T item in lista)
							{
								foreach (string propiedad in nombrePropiedades)
								{
									celda = new Cell();
									celda.Add(new Paragraph(
										(item.GetType().GetProperty(propiedad).GetValue(item)?.ToString() == null ? 
										"" : item.GetType().GetProperty(propiedad).GetValue(item)?.ToString())));
									table.AddCell(celda);
								}
							}
						}

						document.Add(table);
					}

					document.Close();
					pdfWriter.Close();

					return ms.ToArray();
				}
			}
		}

		

		public byte[] exportarExcelDatos<T>(string[] nombrePropiedades, List<T> lista)
		{
			using (MemoryStream ms = new MemoryStream())
			{
				ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
				using (ExcelPackage ep = new ExcelPackage())
				{
					ep.Workbook.Worksheets.Add("Hoja");
					ExcelWorksheet ew = ep.Workbook.Worksheets[0];

					if (nombrePropiedades != null && nombrePropiedades.Length > 0)
					{

						int fila = 2;
						int col = 1;

						if (lista != null)
						{
							foreach (T item in lista)
							{
								col = 1;
								foreach (string propiedad in nombrePropiedades)
								{
									ew.Cells[fila, col++].Value = item.GetType().GetProperty(propiedad).GetValue(item)?.ToString();
								}
								fila++;
							}
						}

						Dictionary<string, string> diccionario = cm.TypeDescriptor.GetProperties(typeof(T)).Cast<cm.PropertyDescriptor>()
							.ToDictionary(p => p.Name, p => p.DisplayName);

						for (int i = 0; i < nombrePropiedades.Length; i++)
						{
							ew.Cells[1, i + 1].Value = diccionario[nombrePropiedades[i]];
							ew.Column(i + 1).AutoFit();
						}
					}

					ep.SaveAs(ms);
					byte[] buffer = ms.ToArray();
					return buffer;
				}
			}
		}
	}
}
