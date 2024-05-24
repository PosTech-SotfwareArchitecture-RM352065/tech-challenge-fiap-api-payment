*** Settings ***
Resource          ../resources/keywords.robot
Resource          ../api_tests.robot

*** Test Cases ***
Ao realizar um novo pedido
    [Documentation]  Ao realizar um login com usuário e senha válidos
    Dado que eu tenho um novo pedido
    Realizar a requisição de pagamento
    Então deve retornar status da resposta 200
