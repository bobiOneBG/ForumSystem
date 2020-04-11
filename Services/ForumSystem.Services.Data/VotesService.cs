namespace ForumSystem.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using ForumSystem.Data.Common.Repositories;
    using ForumSystem.Data.Models;

    public class VotesService : IVotesService
    {
        private readonly IRepository<Vote> votesRepository;

        public VotesService(IRepository<Vote> votesRepository)
        {
            this.votesRepository = votesRepository;
        }

        public int GetVotes(int postId)
        {
            var votes = this.votesRepository.All()
                .Where(v => v.PostId == postId).Sum(x => (int)x.Type);

            return votes;
        }

        public async Task VoteAsync(int postId, string userId, bool isUpVote)
        {
            var vote = this.votesRepository.All()
                .FirstOrDefault(v => v.PostId == postId && v.UserId == userId);

            if (vote != null)
            {
                vote.Type = isUpVote ? VoteType.UpVote : VoteType.DownVote;
            }
            else
            {
                vote = new Vote
                {
                    UserId = userId,
                    PostId = postId,
                    Type = isUpVote ?
                        VoteType.UpVote :
                        VoteType.DownVote,
                };

                await this.votesRepository.AddAsync(vote);
            }

            await this.votesRepository.SaveChangesAsync();
        }
    }
}
