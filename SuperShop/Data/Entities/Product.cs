﻿using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data;
using static System.Net.WebRequestMethods;

namespace SuperShop.Data.Entities
{
    public class Product : IEntity  // "IEntity" foi adicionado quando criamos o repositório genérico - Video 8
    {

        public int Id { get; set; } // Ao colocarmos "Id" já é criado como chave primária.
                                    // Caso o nome seja outro, por exemplo "IdProduct" deve-se colocar um [KEY] acima da propriedade para adicionar using "DataAnnotations"

        [Required] // Obriga o preenchimento do campo
        [MaxLength(50, ErrorMessage ="The field {0} can contain {1} characters lenght.")] // A mensagem de erro não aparece, pois ele bloqueia conforme o MaxLenght
        public string Name { get; set; }

        [DisplayFormat(DataFormatString = "{0:C2}", ApplyFormatInEditMode = false)] // O ApplyFormatEditMode = false faz com que a edição não precise ser como duas casas decimais. 
        public decimal Price { get; set; }

        //[Display(Name = "Image")] // Aqui é a forma como esse campo aparecerá na página Web. Neste caso "Image"
        //public string ImageUrl { get; set; } // RETIRADO APÓS O BOLB NO AZURE

        [Display(Name = "Image")]
        public Guid ImageId { get; set; }

        [Display(Name = "Last Purchase")]
        public DateTime? LastPurchase { get; set; } // O "?" faz com que NÃO seja obrigatório o preenchimento

        [Display(Name = "Last Sale")]
        public DateTime? LastSale { get; set; } // O "?" faz com que NÃO seja obrigatório o preenchimento
         
        [Display(Name = "Is Available")]
        public bool IsAvailable { get; set; }

        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = false)] // O ApplyFormatEditMode = false faz com que a edição não precise ser como duas casas decimais. 
        public double Stock { get; set; } 

        public User User { get; set; }

        public string ImageFullPath => ImageId == Guid.Empty
            ? $"/images/noimage.png" /*https://localhost:44391*/ // CAMINHO RELATIVO PARA AS IMAGENS
            : $"https://supershop.blob.core.windows.net/products/{ImageId}";





        // RETIRADO APÓS O BLOB NO AZURE
        //{
        //    get
        //    {
        //        if (string.IsNullOrEmpty(ImageUrl))
        //        {
        //            return null;
        //        }

        //        return $"https://localhost:44391{ImageUrl.Substring(1)}";
        //    }
        //}
    }
}
