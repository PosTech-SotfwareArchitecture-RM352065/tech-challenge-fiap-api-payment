*** Settings ***
Resource          resources/keywords.robot

*** Variables ***
${API_URL}        https://sanduba-payment-function.azurewebsites.net/api

*** Test Cases ***
Ao realizar um login com usuário e senha válidos
    [Documentation]  Cenário: Ao realizar um login com usuário e senha válidos
    Dado que eu tenho um novo pedido
    Realizar a requisição de pagamento
    Então deve retornar status da resposta 200
