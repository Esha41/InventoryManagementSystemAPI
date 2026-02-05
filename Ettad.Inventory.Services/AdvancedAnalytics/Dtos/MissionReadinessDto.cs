namespace Ettad.Inventory.Service.AdvancedAnalytics.Dtos
{
    /// <summary>
    /// Mission Readiness Rate KPI DTO
    /// </summary>
    public class MissionReadinessDto
    {
        /// <summary>
        /// Overall mission readiness percentage
        /// </summary>
        public decimal ReadinessRate { get; set; }

        /// <summary>
        /// Total number of units/assets
        /// </summary>
        public int TotalUnits { get; set; }

        /// <summary>
        /// Number of mission-ready units
        /// </summary>
        public int MissionReadyUnits { get; set; }

        /// <summary>
        /// Number of units under maintenance
        /// </summary>
        public int UnderMaintenanceUnits { get; set; }

        /// <summary>
        /// Number of units unavailable
        /// </summary>
        public int UnavailableUnits { get; set; }

        /// <summary>
        /// Status indicator: Green (≥95%), Amber (85-94%), Red (<85%)
        /// </summary>
        public string Status { get; set; } = string.Empty;

        /// <summary>
        /// Trend data for the last N days
        /// </summary>
        public List<MissionReadinessTrendDto> TrendData { get; set; } = new();

        /// <summary>
        /// Breakdown by unit type/category
        /// </summary>
        public List<MissionReadinessByCategoryDto> ByCategory { get; set; } = new();
    }

    /// <summary>
    /// Mission Readiness Trend Data Point
    /// </summary>
    public class MissionReadinessTrendDto
    {
        public DateTime Date { get; set; }
        public decimal ReadinessRate { get; set; }
        public int TotalUnits { get; set; }
        public int MissionReadyUnits { get; set; }
    }

    /// <summary>
    /// Mission Readiness by Category
    /// </summary>
    public class MissionReadinessByCategoryDto
    {
        public string CategoryName { get; set; } = string.Empty;
        public int TotalUnits { get; set; }
        public int MissionReadyUnits { get; set; }
        public decimal ReadinessRate { get; set; }
    }
}
