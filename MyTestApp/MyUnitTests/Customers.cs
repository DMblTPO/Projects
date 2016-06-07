using System;
using System.Collections.Generic;

namespace MyUnitTests
{

    class CustomerDb
    {
        public string TenantName { get; set; }
        public Guid? TenantId { get; set; }
        public string PrimaryDomain { get; set; }
        public string Comments { get; set; }
    }

    class Customer
    {
        public Guid TenantId { get; set; }
        public List<Domain> Domains { get; set; }
    }

    class Domain
    {
        public string Name { get; set; }
    }

    static partial class Data
    {
        public static List<Customer> IniCspCustomers
        {
            get
            {
                return new List<Customer>
                {
                    new Customer
                    {
                        TenantId = Guid.Parse("7eb5410f-25f9-4232-8229-48abd6017687"),
                        Domains = new List<Domain> {new Domain {Name = "afterreleasecust11.onmicrosoft.com"},}
                    },
                    new Customer
                    {
                        TenantId = Guid.Parse("2d42a46f-719b-464c-9743-1d48d1881a20"),
                        Domains = new List<Domain> {new Domain {Name = "afterreleasepartsdd.onmicrosoft.com"},}
                    },
                    new Customer
                    {
                        TenantId = Guid.Parse("7605eed4-1131-4353-88bc-692448a2092a"),
                        Domains = new List<Domain> {new Domain {Name = "agavrashenkoTest.onmicrosoft.com"},}
                    },
                    new Customer
                    {
                        TenantId = Guid.Parse("73a84303-7217-45e9-a2b6-aa64467e0639"),
                        Domains =
                            new List<Domain>
                            {
                                new Domain {Name = "alex3058.onmicrosoft.com"},
                                new Domain {Name = "alex3058.com"},
                            }
                    },
                    new Customer
                    {
                        TenantId = Guid.Parse("eea26953-1133-4d1b-bdee-c4d61b08c817"),
                        Domains =
                            new List<Domain>
                            {
                                new Domain {Name = "boscust.onmicrosoft.com"},
                                new Domain {Name = "boscust.net"},
                            }
                    },
                    new Customer
                    {
                        TenantId = Guid.Parse("af944d90-cec8-4dff-a155-64b3478bb56a"),
                        Domains =
                            new List<Domain>
                            {
                                new Domain {Name = "GIOsCustomCloudSolutions.com"},
                                new Domain {Name = "CustomCloudSolutions.onmicrosoft.com"},
                            }
                    },
                    new Customer {TenantId = Guid.Parse("275665e5-7334-4f3b-9c47-2f93bea5751c"), Domains = null},
                };
            }
        }

        public static List<CustomerDb> IniDbCustomers
        {
            get
            {
                return new List<CustomerDb>
                {
                    new CustomerDb
                    {
                        TenantId = Guid.Parse("eea26953-1133-4d1b-bdee-c4d61b08c817"),
                        PrimaryDomain = "boscust.net",
                        TenantName = null,
                        Comments = "#1#2"
                    },
                    new CustomerDb
                    {
                        TenantId = Guid.Parse("2d42a46f-719b-464c-9743-1d48d1881a20"),
                        PrimaryDomain = null,
                        TenantName = "afterreleasepartsdd.onmicrosoft.com",
                        Comments = "#1#2"
                    },
                    new CustomerDb
                    {
                        TenantId = Guid.Parse("af944d90-cec8-4dff-a155-64b3478bb56a"),
                        PrimaryDomain = "GIOsCustomCloudSolutions.com",
                        TenantName = "CustomCloudSolutions.onmicrosoft.com",
                        Comments = "#2"
                    },
                    new CustomerDb
                    {
                        TenantId = Guid.Parse("275665e5-7334-4f3b-9c47-2f93bea5751c"),
                        PrimaryDomain = null,
                        TenantName = null,
                        Comments = "#2"
                    },
                };
            }
        }
    }

}
