using Microsoft.AspNetCore.Mvc;
using Jwt.Entity;
using Jwt.ServiceContracts;
using Jwt.Dto;
using AutoMapper;
using System.Collections.Generic;
using Jwt.RepositoryContracts;
using Jwt.Helpers;

[Route("api/[controller]")]
[ApiController]
public class ClienteController : ControllerBase
{
    private readonly IClienteRepository _clienteRepository;
    private readonly IMapper _mapper;

    public ClienteController(IClienteRepository clienteRepository, IMapper mapper)
    {
        _clienteRepository = clienteRepository;
        _mapper = mapper;
    }

    // POST api/cliente
    [HttpPost]
    public ActionResult<ClienteResponse> Register(ClienteRequest clienteRequest)
    {
        try
        {
            var cliente = _mapper.Map<Cliente>(clienteRequest);
            string plainPassword = cliente.Password;
            string salt = Security.GenerateSalt();
            string hashedPassword = Security.GenerateHashedPassword(plainPassword, salt);
            cliente.PasswordSalt = salt;
            cliente.Password = hashedPassword;
            var createdCliente = _clienteRepository.Create(cliente);
            var clienteResponse = _mapper.Map<ClienteResponse>(createdCliente);
            return CreatedAtAction(nameof(GetByEmail), new { email = createdCliente.Email }, clienteResponse);
        }
        catch
        {
            return BadRequest("Error occurred while creating the client.");
        }
    }

    // DELETE api/cliente
    [HttpDelete]
    public ActionResult DeleteCliente(Cliente cliente)
    {
        try
        {
            _clienteRepository.Delete(cliente);
            return Ok("Client deleted successfully.");
        }
        catch
        {
            return BadRequest("Error occurred while deleting the client.");
        }
    }

    // GET api/cliente
    [HttpGet]
    public ActionResult<List<ClienteResponse>> GetAllClientes()
    {
        var clientes = _clienteRepository.GetAll();
        var clienteResponses = _mapper.Map<List<ClienteResponse>>(clientes);
        return clienteResponses;
    }

    
    [HttpPost("{email}")]
    public ActionResult<ClienteResponse> GetByEmail(string email)
    {
        var cliente = _clienteRepository.GetByEmail(email);
        if (cliente == null)
        {
            return NotFound("Client not found.");
        }
        var clienteResponse = _mapper.Map<ClienteResponse>(cliente);
        return clienteResponse;
    }

    [HttpPost("login")]

    public ActionResult<ClienteResponse> Login(ClienteRequest clienteRequest)
    {
        var cliente = _clienteRepository.GetByEmail(clienteRequest.Email);
        if (cliente == null)
        {
            return NotFound("Invalid Credential.");
        }
        string hashedPassword = Security.GenerateHashedPassword(clienteRequest.Password, cliente.PasswordSalt);
        if (cliente.Password != hashedPassword)
        {
            return BadRequest("Invalid Credential.");
        }
        var clienteResponse = _mapper.Map<ClienteResponse>(cliente);
        return clienteResponse;
    }
}
