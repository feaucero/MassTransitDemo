# MassTransit - Saga State Machine Demo

Requisitos para execução da demo:

* RabbitMQ
* Sql Server

Imagens docker recomendadas:

```docker run -d -p 15672:15672 -p 5672:5672 --name rabbitmq rabbitmq:3-management ```

```docker run -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=SuaSenha" -p 1433:1433 -d mcr.microsoft.com/mssql/server:2022-latest ```

Execute o script ``` Script\create-table.sql ``` no banco Sql Server

Configure as senhas/endereço do rabbitMQ/SqlServer encontrados nos arquivos ```appsettings.json``` dos projetos de acordo com suas necessidades.

Execute o build da solution.

Execute os aplicativos abaixo para testar a demo:
* MassTransitDemo.Orchestration - Maquina de estado que vai orquestrar os microserviços
* MassTransitDemo.StockService - Microserviço de demonstração que processa itens gerados pelo POST.
* MassTransitDemo.PaymentService - Microserviço de demonstração que processa itens que foram processados pelo MassTransitDemo.StockService.
* MassTransitDemo.FinishOrderService - Microserviço que finaliza os itens.
* MassTransitDemo.API - API que oferece o endpoint POST ``` order ``` que insere um pedido para ser processado pela saga.