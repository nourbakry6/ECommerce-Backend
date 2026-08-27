using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Application.DTO
{
    public class CategoryDetailsDTO
    {
    public int id{  get; set; }
    public string Name{  get; set; }
    public List<ProductDTO> products { get; set; }
    }
}
