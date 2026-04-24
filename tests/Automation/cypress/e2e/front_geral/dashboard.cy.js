describe('dashboard', () => {
  it('acessa tela inicial', () => {
    cy.visit('/') // simplesmente acessa o Dashboard
    cy.url().should('eq', 'http://localhost:5173/') // verifica se de fato fui direcionado para tela inicial
  })
  it('verifica dados visiveis', ()=> {
  cy.visit('/')
  cy.get('.text-3xl').should('be.visible') // verifica mensagem de 'Bem-vindo!'
  cy.get('.from-emerald-500 > :nth-child(2) > .text-sm').should('be.visible') // verifica mensagens dos cards
  cy.get('.from-emerald-500 > :nth-child(2) > .text-sm').should('be.visible') // ''
  cy.get('.from-orange-500 > :nth-child(2) > .text-sm').should('be.visible') // ''
  cy.get('aside.card > .text-lg').should('be.visible') // verifica existência do gráfico
  cy.get('.px-5').click() // realiza o clique para módulo de transações
  cy.url().should('eq', 'http://localhost:5173/transacoes') // verifica se de fato fui direcionado para o módulo de transações
})
})