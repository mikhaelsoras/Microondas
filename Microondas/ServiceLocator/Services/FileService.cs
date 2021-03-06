﻿using ServicesLocator.Interfaces;
using System;
using System.IO;
using ServicesLocator.Locator;

namespace ServicesLocator.Services
{
    public class FileService : IFileService
    {
        public string GetExePath(string nomeArquivo)
        {
            var caminho = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, nomeArquivo);
            return caminho;
        }

        public string Carregar(string caminho)
        {
            using (StreamReader arquivo = new StreamReader(caminho))
            {
                return arquivo.ReadToEnd();
            }
        }

        public void RegisterService()
        {
            ServiceLocator.Set<IFileService>(this);
        }

        public void Salvar(string caminho, string conteudo)
        {
            using (StreamWriter arquivo = new StreamWriter(caminho))
            {
                arquivo.Write(conteudo);
            }
        }

        public StreamWriter GetStreamWriter(string caminho, bool append = false)
        {
            if (FileExists(caminho))
                return new StreamWriter(caminho, append);
            return null;
        }

        public bool FileExists(string caminho)
        {
            return File.Exists(caminho);
        }

        public void Deletar(string caminho)
        {
            if (FileExists(caminho))
                File.Delete(caminho);
        }
    }
}
