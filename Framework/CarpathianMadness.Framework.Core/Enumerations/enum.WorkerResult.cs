
namespace CarpathianMadness.Framework
{
    public enum WorkerResult : short
    {
        /// <summary>
        /// Worker successfully performed it's task, continue.
        /// </summary>
        Continue = 0,

        /// <summary>
        /// Worker failed to perform it's task, abort.
        /// </summary>
        Abort = 1,

        /// <summary>
        /// Worker requires the task to be added back to the queue.
        /// </summary>
        ReQueue = 2
    }
}