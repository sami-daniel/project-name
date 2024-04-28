﻿using Entidades;
using System.ComponentModel.DataAnnotations;

namespace Servicos.DTO.AddRequests
{
    /// <summary>
    /// Representa um DTO para adicionar um novo endereço.
    /// </summary>
    public class EnderecoAddRequest
    {
        private string uF;
#pragma warning disable CS8618
        /// <summary>
        /// Obtém ou define a sigla do estado.
        /// </summary>
        [Required(ErrorMessage = "A sigla do estado é obrigatória.")]
        [StringLength(2, MinimumLength = 2, ErrorMessage = "A sigla do estado deve ter 2 caracteres.")]
        public string UF { get => uF; set => uF = value.ToUpper(); }

        /// <summary>
        /// Obtém ou define o nome da cidade.
        /// </summary>
        [Required(ErrorMessage = "O nome da cidade é obrigatório.")]
        [StringLength(100, ErrorMessage = "O nome da cidade não pode exceder 100 caracteres.")]
        public string Cidade { get; set; }

        /// <summary>
        /// Obtém ou define o nome do bairro.
        /// </summary>
        [Required(ErrorMessage = "O nome do bairro é obrigatório.")]
        [StringLength(100, ErrorMessage = "O nome do bairro não pode exceder 100 caracteres.")]
        public string Bairro { get; set; }

        /// <summary>
        /// Obtém ou define o nome do logradouro.
        /// </summary>
        [Required(ErrorMessage = "O nome do logradouro é obrigatório.")]
        [StringLength(100, ErrorMessage = "O nome do logradouro não pode exceder 100 caracteres.")]
        public string Logradouro { get; set; }

        /// <summary>
        /// Obtém ou define o número do endereço.
        /// </summary>
        [Required(ErrorMessage = "O número do endereço é obrigatório.")]
        public ushort Numero { get; set; }

        /// <summary>
        /// Obtém ou define o complemento do endereço.
        /// </summary>
        [StringLength(100, ErrorMessage = "O complemento do endereço não pode exceder 100 caracteres.")]
        public string Complemento { get; set; }
        /// <summary>
        /// Converte o objeto atual de EnderecoAddRequest para um novo objeto do tipo Endereco
        /// </summary>
        /// <returns>Um novo objeto do tipo Endereco baseado no objeto atual de EnderecoAddRequest</returns>
#pragma warning restore CS8618
        public Endereco ToEndereco() => new Endereco()
        {
            Uf = UF,
            Cidade = Cidade,
            Bairro = Bairro,
            Logradouro = Logradouro,
            Numero = Numero,
            Complemento = Complemento
        };
    }
}
