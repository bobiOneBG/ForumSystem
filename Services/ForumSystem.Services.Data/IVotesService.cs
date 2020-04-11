namespace ForumSystem.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;

    public interface IVotesService
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="isUpVote"> isUpVote == true - UpVote, else - DownVote. </param>
        Task VoteAsync(int postId, string userId, bool isUpVote);

        int GetVotes(int postId);
    }
}
