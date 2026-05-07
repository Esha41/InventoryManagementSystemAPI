namespace Ettad.Inventory.Service.Assets
{
    internal static class AssetBulkDeletionConstants
    {
        public static class JobStatus
        {
            public const byte Pending = 0;
            public const byte Running = 1;
            public const byte Completed = 2;
            public const byte Failed = 3;
        }

        public static class Scope
        {
            public const byte Batch = 0;
            public const byte Depot = 1;
            public const byte ExplicitIds = 2;
        }

        /// <summary>LINQ Contains batch size staying under SQL Server parameter limits.</summary>
        public const int ExplicitSliceSize = 2000;

        public const int UpdateChunkSize = 10_000;
    }
}
