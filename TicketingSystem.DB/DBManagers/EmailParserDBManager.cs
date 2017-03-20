using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TicketingSystem.DB.IDBManagers;

namespace TicketingSystem.DB.DBManagers
{
    public class EmailParserDBManager : IEmailParserDBManager
    {
        public List<ViewModel.EmailParseRule> GetAllEmailParseRules()
        {
            List<ViewModel.EmailParseRule> _rules = new List<ViewModel.EmailParseRule>();
            using (Database.TicketingSystemDBContext context = new Database.TicketingSystemDBContext())
            {
                foreach (var rule in context.EmailParsingRules.Where(x => x.IsActive == true))
                {
                    _rules.Add(ConvertToViewModelObject(rule));
                }
            }
            return _rules;
        }
        private int ExtractTicketIDFromSubject(string subject)
        {
            string ticketIdString = string.Empty;
            int ticketId = 0;
            Regex regex = new Regex(@"-TKT[0-9]*", RegexOptions.IgnoreCase);
            ticketIdString = new Regex(@"\d+").Match(regex.Match(subject).Value).Value;
            int.TryParse(ticketIdString, out ticketId);
            return ticketId;
        }
        public bool CheckIfValidEmail(ViewModel.Email email)
        {
            bool isValid = true;
            List<ViewModel.EmailParseRule> _rules = GetAllEmailParseRules();
            foreach (var rule in _rules)
            {
                if (isValid && rule.ParseProperty == ViewModel.ParseRuleProperty.To)
                {
                    isValid = CheckIfValidEmailByUserListProperty(email.To, rule);
                }
                if (isValid && rule.ParseProperty == ViewModel.ParseRuleProperty.CC)
                {
                    isValid = CheckIfValidEmailByUserListProperty(email.CC, rule);
                }
                if (isValid && rule.ParseProperty == ViewModel.ParseRuleProperty.From)
                {
                    isValid = CheckIfValidEmailByProperty(email.From.Email, rule);
                }
                if (isValid && rule.ParseProperty == ViewModel.ParseRuleProperty.Subject)
                {
                    isValid = CheckIfValidEmailByProperty(email.Subject, rule, true);
                }
                if (isValid && rule.ParseProperty == ViewModel.ParseRuleProperty.Body)
                {
                    isValid = CheckIfValidEmailByProperty(email.Body, rule, true);
                }
            }
            return isValid;
        }
        private bool CheckIfValidEmailForAllProperty()
        {
            bool isValid = false;

            return isValid;
        }
        private bool CheckIfValidEmailByProperty(dynamic obj, ViewModel.EmailParseRule rule, bool isCaseSensitive = false)
        {
            bool isValid = false;
            switch (rule.ParseType)
            {
                case ViewModel.ParseRuleType.Contains: isValid = isCaseSensitive ? obj.Contains(rule.ParsingRule) : obj.ToLower().Contains(rule.ParsingRule.ToLower()); break;
                case ViewModel.ParseRuleType.DoesNotContains: isValid = isCaseSensitive ? !obj.Contains(rule.ParsingRule) : !obj.ToLower().Contains(rule.ParsingRule.ToLower()); break;
                case ViewModel.ParseRuleType.DoesNotEquals: isValid = isCaseSensitive ? !obj.Equals(rule.ParsingRule) : !obj.ToLower().Equals(rule.ParsingRule.ToLower()); break;
                case ViewModel.ParseRuleType.Equals: isValid = isCaseSensitive ? obj.Equals(rule.ParsingRule) : obj.ToLower().Equals(rule.ParsingRule.ToLower()); break;
                case ViewModel.ParseRuleType.EndsWith: isValid = isCaseSensitive ? obj.EndsWith(rule.ParsingRule) : obj.ToLower().EndsWith(rule.ParsingRule.ToLower()); break;
                case ViewModel.ParseRuleType.StartsWith: isValid = isCaseSensitive ? obj.StartsWith(rule.ParsingRule) : obj.ToLower().StartsWith(rule.ParsingRule.ToLower()); break;
            }
            return !isValid;
        }
        private bool CheckIfValidEmailByUserListProperty(List<ViewModel.User> users, ViewModel.EmailParseRule rule, bool isCaseSensitive = false)
        {
            bool isValid = false;
            switch (rule.ParseType)
            {
                case ViewModel.ParseRuleType.Contains: isValid = isCaseSensitive ? users.Any(x => x.Email.Contains(rule.ParsingRule)) : users.Any(x => x.Email.ToLower().Contains(rule.ParsingRule.ToLower())); break;
                case ViewModel.ParseRuleType.DoesNotContains: isValid = isCaseSensitive ? !users.Any(x => x.Email.Contains(rule.ParsingRule)) : !users.Any(x => x.Email.ToLower().Contains(rule.ParsingRule.ToLower())); break;
                case ViewModel.ParseRuleType.DoesNotEquals: isValid = isCaseSensitive ? !users.Any(x => x.Email.Equals(rule.ParsingRule)) : !users.Any(x => x.Email.ToLower().Equals(rule.ParsingRule.ToLower())); break;
                case ViewModel.ParseRuleType.Equals: isValid = isCaseSensitive ? users.Any(x => x.Email.Equals(rule.ParsingRule)) : users.Any(x => x.Email.ToLower().Equals(rule.ParsingRule.ToLower())); break;
                case ViewModel.ParseRuleType.EndsWith: isValid = isCaseSensitive ? users.Any(x => x.Email.EndsWith(rule.ParsingRule)) : users.Any(x => x.Email.ToLower().EndsWith(rule.ParsingRule.ToLower())); break;
                case ViewModel.ParseRuleType.StartsWith: isValid = users.Any(x => x.Email.ToLower().StartsWith(rule.ParsingRule)); break;
                case ViewModel.ParseRuleType.IsEmpty: isValid = users.Count == 0; break;
            }
            return !isValid;
        }

        public int CheckIfEmailIsForAlreadyCreatedTicketFromSubjectAndReturnTicketId(string subject)
        {
            return ExtractTicketIDFromSubject(subject);
        }
        private ViewModel.EmailParseRule ConvertToViewModelObject(Database.EmailParsingRule rule)
        {
            if (rule == null)
            {
                return null;
            }
            ViewModel.EmailParseRule _rule = new ViewModel.EmailParseRule();
            _rule.ID = rule.ID;
            _rule.IsActive = rule.IsActive;

            var _ruleProperty = ViewModel.ParseRuleProperty.Common;
            Enum.TryParse<ViewModel.ParseRuleProperty>(rule.ParseProperty, out _ruleProperty);
            _rule.ParseProperty = _ruleProperty;
            _rule.ParsingRule = rule.ParseRule;

            var _ruleType = ViewModel.ParseRuleType.Equals;
            Enum.TryParse<ViewModel.ParseRuleType>(rule.ParseType, out _ruleType);
            _rule.ParseType = _ruleType;

            return _rule;
        }
    }
}
