describe('example to-do app', () => {
    beforeEach(() => {

      cy.visit('http://localhost:50596/')
    })

    it('displays two todo items by default', () => {
        cy.get('#z').click();
        cy.get('input[name=NCorreo]').type('Calidad@correo.com');
        cy.get('input[name=NContrasena]').type('calidad123');
        cy.contains('Ingresar').click();
      })
})