describe('Pessoas - CRUD integração API', () => {

  it('Deve criar, buscar, atualizar e deletar uma pessoa', () => {

    let pessoaId

    const nomeInicial = `Pessoa_${Date.now()}`
    const nomeAtualizado = `Pessoa_Atualizada_${Date.now()}`
    const dataNascimento = '2000-01-01'

    //  criar (POST)
    cy.request({
      method: 'POST',
      url: `${Cypress.env('apiUrl')}/api/v1.0/pessoas`,
      body: {
        nome: nomeInicial,
        dataNascimento: dataNascimento
      }
    }).then((res) => {

      expect(res.status).to.eq(201)

      pessoaId = res.body.id

      // validação extra no front integracao
      cy.visit('/pessoas')
      cy.contains('button', '1').click()
      cy.buscarItemPaginado(nomeInicial) 

      // busca (GET por ID)
      cy.request({
        method: 'GET',
        url: `${Cypress.env('apiUrl')}/api/v1.0/pessoas/${pessoaId}`
      }).then((resGet) => {

        expect(resGet.status).to.eq(200)
        expect(resGet.body.id).to.eq(pessoaId)
        expect(resGet.body.nome).to.eq(nomeInicial)

        // atualiza pessoa criada (PUT)
        cy.request({
          method: 'PUT',
          url: `${Cypress.env('apiUrl')}/api/v1.0/pessoas/${pessoaId}`,
          body: {
            nome: nomeAtualizado,
            dataNascimento: dataNascimento
          }
        }).then((resPut) => {

          expect(resPut.status).to.be.oneOf([200, 204])

          // verifica update da pessoa
          cy.request({
            method: 'GET',
            url: `${Cypress.env('apiUrl')}/api/v1.0/pessoas/${pessoaId}`
          }).then((resUpdated) => {

            expect(resUpdated.body.nome).to.eq(nomeAtualizado)

            // deleta pessoa criada (DELETE)
            cy.request({
              method: 'DELETE',
              url: `${Cypress.env('apiUrl')}/api/v1.0/pessoas/${pessoaId}`
            }).then((resDelete) => {

              expect(resDelete.status).to.be.oneOf([200, 204])

              // verifica se realmente foi deletada tentando buscar novamente (GET por ID)
              cy.request({
                method: 'GET',
                url: `${Cypress.env('apiUrl')}/api/v1.0/pessoas/${pessoaId}`,
                failOnStatusCode: false // importante
              }).then((resFinal) => {

                expect(resFinal.status).to.eq(404)
              })
            })
          })
        })
      })
    })
  })
})