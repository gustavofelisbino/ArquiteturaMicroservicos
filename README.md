# Propósito do Sistema #

O sistema tem como propósito gerenciar de forma integrada às operações de uma oficina mecânica, permitindo o cadastro e controle de clientes, veículos e ordens de serviço. 

A solução foi projetada com arquitetura baseada em microsserviços, garantindo modularidade, escalabilidade e independência entre os módulos.

O objetivo principal é otimizar o fluxo de atendimento da oficina, desde o registro do cliente e do carro até a emissão e acompanhamento das ordens de serviço.

# Usuários do Sistema

- Atendentes da oficina: responsáveis por cadastrar clientes, veículos e abrir novas ordens de serviço.
- Mecânicos: consultam as ordens de serviço em andamento e atualizam o status das atividades.

# Requisitos Funcionais
- Cadastro de Clientes / O sistema deve permitir o cadastro, edição, consulta e exclusão de clientes.
- Cadastro de Veículos / O sistema deve permitir o registro de veículos vinculados a clientes, com informações como marca, modelo, ano e placa.
- Cadastro de Ordens de Serviço / O sistema deve permitir a criação de ordens de serviço associadas a um veículo e cliente, incluindo descrição do problema, data de entrada e status.
- Atualização de Status / O sistema deve permitir a atualização do status da ordem (ex: em andamento, concluída, entregue). Ex: Assim que colocar a ordem em andamento, atualizar o carro para “indisponível”

- Consulta de Dados / O sistema deve permitir a listagem e consulta de clientes, veículos e ordens de serviço. Ex: Quando buscar o cliente, trazer o histórico de ordens.
- Comunicação entre Microsserviços O sistema deve integrar os três microsserviços (Clientes, Carros e Ordens de Serviço) por meio de APIs REST, permitindo o compartilhamento de dados.

# Descritivo Técnico dos Microsserviços
O sistema é composto por três microsserviços independentes, cada um com banco de dados próprio e comunicação via APIs REST.
# 1. Microsserviço de Clientes
- Função: Gerenciar informações de clientes.
# Responsabilidades: 
- Criar, ler, atualizar e excluir clientes (CRUD).
- Fornecer dados de clientes para o microsserviço de ordens de serviço.

# Endpoints principais:

- POST /clientes → Cadastrar cliente
- GET /clientes → Listar clientes
- GET /clientes/{id} → Buscar cliente específico
- PUT /clientes/{id} → Atualizar cliente
- DELETE /clientes/{id} → Excluir cliente

# 2. Microsserviço de Carros
- Função: Gerenciar informações sobre veículos da oficina.
# Responsabilidades:
- Registrar carros vinculados a clientes.
- Consultar e atualizar informações dos veículos.
- Disponibilizar dados de veículos para o microsserviço de ordens de serviço.

# Endpoints principais:

- POST /carros → Cadastrar carro
- GET /carros → Listar carros
- GET /carros/{id} → Buscar carro específico
- PUT /carros/{id} → Atualizar carro
- DELETE /carros/{id} → Excluir carro

# 3. Microsserviço de Ordens de Serviço
- Função: Controlar todas as ordens de serviço da oficina.
# Responsabilidades:

- Criar e gerenciar ordens de serviço associando cliente e veículo.
- Consultar status e histórico das ordens.
- Integrar dados com os microsserviços de Clientes e Carros.

# Endpoints principais:

- POST /OrdensServico → Criar ordem de serviço
- GET /OrdensServico → Listar ordens
- GET /OrdensServico/{id} → Buscar ordem específica
- GET /OrdensServico/cliente/{id}/historico → Mostra ordens de serviço vinculadas ao cliente
= PUT /OrdensServico/{id} → Atualizar status ou informações da ordem
= DELETE /OrdensServico/{id} → Excluir ordem de serviço
= PATCH/OrdensServico/{id}/status → Atualiza status da ordem de serviço do cliente

# Integração entre Microsserviços
- A comunicação entre os três microsserviços é feita por requisições HTTP (REST).
- O microsserviço de Ordens de Serviço consome as APIs de Clientes e Carros para obter dados necessários ao cadastro e exibição das ordens.
