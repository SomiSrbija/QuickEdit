using LibGit2Sharp;
using System;
using System.Linq;

namespace QuickEdit
{
    public class GitIntegration
    {
        private Repository repository;

        public GitIntegration(string repoPath)
        {
            repository = new Repository(repoPath);
        }

        public void Commit(string message, Signature author, Signature committer)
        {
            Commands.Stage(repository, "*");
            repository.Commit(message, author, committer);
        }

        public void Push()
        {
            var options = new PushOptions();
            var remote = repository.Network.Remotes["origin"];
            repository.Network.Push(remote, @"refs/heads/main", options);
        }

        public void Pull()
        {
            var options = new PullOptions();
            var signature = repository.Config.BuildSignature(DateTimeOffset.Now);
            Commands.Pull(repository, signature, options);
        }

        public string GetDiff()
        {
            var diff = repository.Diff.Compare<Patch>(repository.Head.Tip.Tree, DiffTargets.WorkingDirectory);
            return diff.Content;
        }

        public string GetStatus()
        {
            var status = repository.RetrieveStatus();
            return string.Join(Environment.NewLine, status.Select(x => x.FilePath + " " + x.State));
        }
    }
}
