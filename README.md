# Validação de Documentos com Azure Document Intelligence

Este projeto é uma aplicação em C# utilizando .NET 9 e Minimal API para validar documentos usando o Azure Document Intelligence.

## Índice

- [Introdução](#introdução)
- [Pré-requisitos](#pré-requisitos)
- [Instalação](#instalação)
- [Configuração](#configuração)
- [Uso](#uso)
- [Contribuição](#contribuição)
- [Licença](#licença)

## Introdução

Esta aplicação demonstra como criar uma API mínima em .NET 9 para validar documentos utilizando o Azure Document Intelligence. A API permite o upload de documentos e valida suas informações usando os serviços de inteligência de documentos da Azure.

## Pré-requisitos

- [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- [Conta Azure](https://azure.microsoft.com/)
- [Azure Document Intelligence](https://azure.microsoft.com/services/document-intelligence/)

## Instalação

1. Clone este repositório:

    ```bash
    git clone https://github.com/seu-usuario/seu-repositorio.git
    cd seu-repositorio
    ```

2. Restaure as dependências do projeto:

    ```bash
    dotnet restore
    ```

## Configuração

1. Configure as credenciais do Azure Document Intelligence no arquivo `appsettings.json`:

    ```json
    {
      "Azure": {
        "DocumentIntelligence": {
          "Endpoint": "https://seu-endpoint.cognitiveservices.azure.com/",
          "ApiKey": "sua-chave-api"
        }
      }
    }
    ```

## Uso

1. Execute a aplicação:

    ```bash
    dotnet run
    ```

2. Envie um documento para validação usando um cliente HTTP, como o `curl`:

    ```bash
    curl -F "file=@caminho/para/seu/documento.pdf" http://localhost:5000/validate
    ```

## Contribuição

Contribuições são bem-vindas! Sinta-se à vontade para abrir issues e pull requests.

## Licença

Este projeto está licenciado sob a Licença MIT. Veja o arquivo [LICENSE](LICENSE) para mais detalhes.
