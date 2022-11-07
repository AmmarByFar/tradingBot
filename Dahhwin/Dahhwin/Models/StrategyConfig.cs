using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dahhwin.Models
{
    class StrategyConfig
    {
        public string template_version { get; set; } = "24";
        public string strategy { get; set; } = "pure_market_making";
        public string exchange { get; set; } = "binance_paper_trade";
        public string market { get; set; } = "BTC-USDT";
        public string bid_spread { get; set; }
        public string ask_spread { get; set; }
        public string minimum_spread { get; set; } = "-100";
        public string order_refresh_time { get; set; } = "300";
        public string max_order_age { get; set; } = "1800";
        public string order_refresh_tolerance_pct { get; set; } = "0.0";
        public string order_amount { get; set; }
        public string price_ceiling { get; set; } = "-1.0";
        public string price_floor { get; set; } = "-1.0";
        public string price_band_refresh_time { get; set; } = "86400";
        public string ping_pong_enabled { get; set; } = "false";
        public string inventory_skew_enabled { get; set; } = "false";
        public string inventory_target_base_pct { get; set; } = "50.0";
        public string inventory_range_multiplier { get; set; } = "1.0";
        public string inventory_price { get; set; } = "1.0";
        public string order_levels { get; set; } = "1";
        public string order_level_amount { get; set; } = "0";
        public string order_level_spread { get; set; } = "1.0";
        public string filled_order_delay { get; set; } = "60";
        public string hanging_orders_enabled { get; set; } = "false";
        public string hanging_orders_cancel_pct { get; set; } = "10.0";
        public string order_optimization_enabled { get; set; } = "false";
        public string ask_order_optimization_depth { get; set; } = "0";
        public string bid_order_optimization_depth { get; set; } = "0";
        public string add_transaction_costs { get; set; } = "false";
        public string price_type { get; set; } = "mid_price";
        public string price_source_exchnge { get; set; } = "";
        public string price_source_market { get; set; } = "";
        public string price_source_custom_api { get; set; } = "";
        public string custom_api_update_interval { get; set; } = "5.0";
        public string take_if_crossed { get; set; } = "";
        public string order_override { get; set; } = "";
        public string split_order_levels_enabled { get; set; } = "false";
        public string bid_order_level_spreads { get; set; } = "";
        public string ask_order_level_spreads { get; set; } = "";
        public string bid_order_level_amounts { get; set; } = "";
        public string ask_order_level_amounts { get; set; } = "";
        public string should_wait_order_cancel_confirmation { get; set; } = "true";
    }
}
