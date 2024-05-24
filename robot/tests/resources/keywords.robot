*** Settings ***
Library     RequestsLibrary
Library     OperatingSystem
Library     Collections

*** Variables ***
${API_URL}        https://sanduba-payment-function.azurewebsites.net/api
${VALID_PAYLOAD}    ${CURDIR}/../payloads/new_payment.json

*** Keywords ***
Dado que eu tenho um novo pedido
    [Documentation]  Define a URL da API que será testada
    ${payload}=    Get File    ${VALID_PAYLOAD}
    Log    Payload: ${payload}
    Set Suite Variable  ${payload}

Realizar a requisição de pagamento
    [Documentation]  Realiza uma requisição GET para a API
    Create Session    mysession    ${API_URL}
    ${response}=    POST On Session    mysession    PaymentCreation    ${payload}
    Log    Status Code: ${response.status_code}
    Log    Response Content: ${response.content}
    Set Suite Variable  ${response}

Então deve retornar status da resposta 200
    [Documentation]  Verifica se o status da resposta é 200
    Should Be Equal As Numbers    ${response.status_code}    200