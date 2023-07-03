using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Octokit;
using Octokit.Webhooks;
using Octokit.Models;
using Octokit.Webhooks.Events;
using Swashbuckle.AspNetCore.Annotations;

namespace APIWebGithub.Controllers
{
    [ApiController]
    [Route("[controller]")]
        public class GitHubController : ControllerBase
        {
            private readonly GitHubClient _gitHubClient;

            public GitHubController()
            {
            // Configurar autenticação usando o token de acesso do GitHub
            var tokenAuth = new Credentials("Token_de_acesso");
                _gitHubClient = new GitHubClient(new ProductHeaderValue("aplicativo"));
                _gitHubClient.Credentials = tokenAuth;
            }

            // Criação de um repositório
            [HttpPost("repos")]
            public async Task<ActionResult<Repository>> CreateRepository(string repoName)
            {
                try
                {
                    var newRepo = new NewRepository(repoName);
                    var repository = await _gitHubClient.Repository.Create(newRepo);

                    return repository;
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }

        // Listar branches de um repositório
        [HttpGet("repos/{owner}/{repo}/branches")]
        public async Task<ActionResult<List<string>>> GetBranches(string owner, string repo)
        {
            try
            {
                var repositoryBranches = await _gitHubClient.Repository.Branch.GetAll(owner, repo);
                var branches = repositoryBranches.Select(branch => branch.Name).ToList();

                return branches;
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // Listar webhooks de um repositório
        [HttpGet("repos/{owner}/{repo}/webhooks")]
        public async Task<ActionResult<List<RepositoryHook>>> GetWebhooks(string owner, string repo)
        {
            try
            {
                var hooks = await _gitHubClient.Repository.Hooks.GetAll(owner, repo);

                return hooks.ToList();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // Adicionar webhook em um repositório
        [HttpPost("repos/{owner}/{repo}/webhooks")]
        public async Task<ActionResult<RepositoryHook>> AddWebhook(string owner, string repo, NewRepositoryHook newWebhook)
        {
            try
            {
                var webhook = await _gitHubClient.Repository.Hooks.Create(owner, repo, newWebhook);

                return webhook;
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // Atualizar uma webhook de um repositório
        [HttpPut("repos/{owner}/{repo}/webhooks/{webhookId}")]
        public async Task<ActionResult<RepositoryHook>> UpdateWebhook(string owner, string repo, int webhookId, EditRepositoryHook webhookUpdate)
        {
            try
            {
                var updatedWebhook = await _gitHubClient.Repository.Hooks.Edit(owner, repo, webhookId, webhookUpdate);

                return updatedWebhook;
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
    }

